using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using BestHTTP;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Template.Defines;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public static class ExtensionException
{
    public static void ShowException(this Exception e)
    {
        var message = string.IsNullOrEmpty(e.Message) ? e.ToString() : e.Message;
        if (message.Contains("A task was canceled"))
            return;
        if (e is AsyncHTTPException httpException)
        {
            message = $"({httpException.StatusCode}) {httpException.Message}";
            if (!string.IsNullOrEmpty(httpException.Content))
            {
                JObject jObj = null;
                try
                {
                    jObj = JObject.Parse(httpException.Content);
                }
                catch (Exception exception)
                {
                }

                if (jObj != null)
                {
                    message += jObj.SelectToken("message") ?? jObj.SelectToken("error.message");
                }
            }

            message = httpException.StatusCode switch
            {
                0 => "Mất kết nối Internet",
                400 => "Đăng nhập bị lỗi",
                _ => message
            };
        }

        // PopupChild.Show("Xảy ra lỗi!\n" + message, "", null, header: "THÔNG BÁO");
        Debug.LogError(e);
    }

    public static CancellationTokenSource CreateCancellationTokenSource()
    {
        CancellationTokenSource cts = default;
        cts?.Dispose();
        cts = new CancellationTokenSource();
        return cts;
    }

    public static void ShowError(this Exception e, bool isShowException = true)
    {
        var message = e.Message;
        if (e is UnityWebRequestException webRequestException)
        {
            message = GameUtils.Parse(webRequestException.Text);
            try
            {
                var data = JsonConvert.DeserializeObject<ModelApiException>(message);
                if (data.code == 401)
                {
                    // message = Localization.Get(TextId.Toast_SessionExpired);
                    message = "Session expired";
                    SpecialExtensionObserver.PostEvent(null, TypeGameEvent.InterruptGame);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                    message = data.message ?? (data.error != null ? data.error.message : "Unknown error");
            }
            catch
            {
            }
        }
        else if (message.StartsWith("{") && message.EndsWith("}"))
        {
            try
            {
                var data = JsonConvert.DeserializeObject<ModelApiException>(message);
                message = data.message ?? data.description ?? (data.error != null ? data.error.message : "Unknown error");
            }
            catch
            {
            }
        }

        if (isShowException)
        {
            ControllerPopup.SetApiLoading(false);
            ControllerPopup.ShowToastError(message);
        }

#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
