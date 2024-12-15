using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if USE_FIREBASE
using Firebase.Crashlytics;
using System.Collections.Generic;
using System.Threading.Tasks;


#endif

#if USE_SENTRY
using Sentry;
#endif

public static class Logger
{
    public static void Log(string message)
    {
        Debug.Log($"<color=#ecf0f1>{message}</color>");
#if USE_FIREBASE
        Crashlytics.Log($"Normal: {message}");
#endif
    }

    public static void SentryDebugLog(string message)
    {
#if USE_SENTRY
        SentrySdk.CaptureMessage(message);
#endif
    }

    public static void LogWarning(string message)
    {
        Debug.LogWarning($"<color=#e67e22>Warning: {message}</color>");
#if USE_FIREBASE
        Crashlytics.Log($"Warning: {message}");
#endif
    }

    public static void LogError(string message)
    {
        Debug.LogError($"<color=#c0392b>Error: {message}</color>");
#if USE_FIREBASE
        Crashlytics.Log($"Error: {message}");
#endif
    }

    public static void LogException(Exception e, Dictionary<string, string> optionalData = null)
    {
        if (e is TaskCanceledException) { return; }
        Debug.LogError(e);
#if USE_SENTRY
        SentrySdk.CaptureException(e, scope =>
        {
            if (optionalData != null)
            {
                foreach (var pair in optionalData)
                {
                    scope.SetTag(pair.Key, pair.Value);
                }

                scope.Level = SentryLevel.Fatal;
            }
        });
#endif
#if USE_FIREBASE
        Crashlytics.LogException(e);
#endif
    }
}
