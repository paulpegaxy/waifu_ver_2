using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

public static class TelegramWebApp
{
#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern string _GetTelegramInitData();

	[DllImport("__Internal")]
	private static extern string _OpenLink(string link);

	[DllImport("__Internal")]
	private static extern string _OpenInvoice(int taskId, string invoice);

	[DllImport("__Internal")]
	private static extern void _CopyToClipboard(string text);

	[DllImport("__Internal")]
	private static extern void _SetLoadingProgress(float progress);

	[DllImport("__Internal")]
	private static extern void _SetLoadingText(string text);

	[DllImport("__Internal")]
	private static extern bool _ShareToStory(string mediaUrl, string text, string widgetUrl, string widgetName);

	[DllImport("__Internal")]
	private static extern bool _IsProduction();

	[DllImport("__Internal")]
	private static extern void _Reload();

	[DllImport("__Internal")]
	private static extern bool _IsMobile();

	[DllImport("__Internal")]
	private static extern string _Platform();
#endif

	private static string _initData = "";
	public static string InitData
	{
		get
		{
#if UNITY_WEBGL
			if (string.IsNullOrEmpty(_initData) && !Application.isEditor) _initData = _GetTelegramInitData();
#endif
			return _initData;
		}
	}

	public static void OpenLink(string link)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        _OpenLink(link);
#else
		Application.OpenURL(link);
#endif
	}

	public static UniTask<bool> OpenInvoice(string invoice)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		var tcs = new UniTaskCompletionSource<bool>();
		var taskId = WebTask.Create(cb =>
		{
			if (cb.success) tcs.TrySetResult(true);
			else tcs.TrySetException(new Exception(cb.data));
		});

		_OpenInvoice(taskId, invoice);

		return tcs.Task;
#endif
		throw new Exception("Not supported in this platform");
	}

	public static void CopyToClipboard(string text)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        _CopyToClipboard(text);
#else
		GUIUtility.systemCopyBuffer = text;
#endif
	}

	public static void SetLoadingProgress(float progress)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        _SetLoadingProgress(progress);
#endif
	}

	public static void SetLoadingText(string text)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        _SetLoadingText(text);
#endif
	}

	public static bool ShareToStory(string mediaUrl, string text, string widgetUrl, string widgetName)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        return _ShareToStory(mediaUrl, text, widgetUrl, widgetName);
#endif
		return false;
	}

	public static bool IsProduction()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		return _IsProduction();
#else
		return false;
#endif
	}

	public static void Reload()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		_Reload();
#endif
	}

	public static bool IsMobile()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        return _IsMobile();
#else
		return false;
#endif
	}

	public static string Platform()
	{
#if UNITY_WEBGL && !UNITY_EDITOR
        return _Platform();
#else
		return "Unity Editor";
#endif
	}
}
