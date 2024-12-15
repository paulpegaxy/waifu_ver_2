using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Supyrb;
using Supyrb.Attributes;
using UnityEngine;

public class WebTaskCommands : WebCommands
{
    [WebCommand(Description = "CallbackWebTask")]
    public void UnityTaskCallBack(string data)
    {
        Debug.Log("CallbackData:"+data);
        var callbackData = JsonConvert.DeserializeObject<WebTaskCallBack>(data);
        if (WebTask.Tasks.TryGetValue(callbackData.taskId, out var callback))
        {
            callback?.Invoke(callbackData);
            WebTask.Tasks.Remove(callbackData.taskId);
        }
    }
}
