using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class WebTaskCallBack
{
    public int taskId;
    public bool success;
    public string data;
}
public class WebTaskException : Exception
{
    public WebTaskException()
    {
    }
    public WebTaskException(string message) : base(message)
    {
    }
    public WebTaskException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
public static class WebTask
{
    public static readonly Dictionary<int, Action<WebTaskCallBack>> Tasks = new();
    public static int Create(Action<WebTaskCallBack> callback=null)
    {
        var taskId = GenerateTaskId();
        Tasks[taskId] = callback;
        return taskId;
    }
    public static UniTask<string> Create(Action<int> syncCall)
    {
        var taskId = GenerateTaskId();
        var tcs = new UniTaskCompletionSource<string>();
        Tasks[taskId] = cb =>
        {
            if (cb.success) tcs.TrySetResult(cb.data);
            else tcs.TrySetException(new WebTaskException(cb.data));
        };
        syncCall?.Invoke(taskId);
        return tcs.Task;
    }

    public static async UniTask<T> Create<T>(Action<int> syncCall) =>
        JsonConvert.DeserializeObject<T>(await Create(syncCall));
    private static int GenerateTaskId()
    {
        return Guid.NewGuid().GetHashCode();
    }
}