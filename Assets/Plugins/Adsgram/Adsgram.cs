using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
[Serializable]
public class ShowResult
{
    [Serializable]
    public enum State
    {
        load,
        render,
        playing,
        destroy
    }
    public bool done;
    public string description;
    public State state;
    public bool error;
}

public static class Adsgram
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void _AdsgramShow(int taskId, long blockId);
#endif
    public static UniTask<ShowResult> Show(long blockId)
        => WebTask.Create<ShowResult>(taskId => _AdsgramShow(taskId, blockId));
}
