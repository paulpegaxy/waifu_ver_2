using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildAutomation : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    [MenuItem("Environment/Build Development")]
    public static void BuildDevelopment()
    {
        var isProduction = IsProduction();
        if (isProduction) SwitchToDevelopment();

        LogDEV();
        Build();

        if (isProduction) SwitchToProduction();
    }

    [MenuItem("Environment/Build Production")]
    public static void BuildProduction()
    {
        var isProduction = IsProduction();
        if (!isProduction) SwitchToProduction();

        LogProduction();
        Build();

        if (!isProduction) SwitchToDevelopment();
    }

    [MenuItem("Environment/Build Production TEST")]
    public static void BuildProductionTest()
    {
        var isProduction = IsProduction();
        if (!isProduction) SwitchToProduction();
        var isProdTest = IsTestProductionEnv();
        if (!isProdTest) SwitchToProductionTest();

        LogDEV();
        Build();


        if (!isProduction) SwitchToDevelopment();
    }

    [MenuItem("Environment/Switch to Development")]
    public static void SwitchToDevelopment()
    {
        if (!IsProduction()) return;

        PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL, out string[] arrStrDefine);
        var listString = arrStrDefine.ToList();
        listString.Remove("PRODUCTION_BUILD");
        var finalString = string.Join(";", listString);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, finalString);

        // var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
        // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, symbols.Replace(";PRODUCTION_BUILD", ""));
        // PlayerSettings.SetTemplateCustomValue("PRODUCTION_BUILD", "0");
    }

    [MenuItem("Environment/Switch to Production")]
    public static void SwitchToProduction()
    {
        if (IsProduction()) return;

        PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL, out string[] arrStrDefine);
        var listString = arrStrDefine.ToList();
        if (listString.Exists(x => x.Equals("TEST_PRODUCTION_BUILD")))
        {
            listString.Remove("TEST_PRODUCTION_BUILD");
        }

        listString.Add("PRODUCTION_BUILD");
        var finalString = string.Join(";", listString);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, finalString);

        // var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
        // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, $"{symbols};PRODUCTION_BUILD");
        // PlayerSettings.SetTemplateCustomValue("PRODUCTION_BUILD", "1");
    }

    [MenuItem("Environment/Switch to Production TEST")]
    public static void SwitchToProductionTest()
    {
        if (IsTestProductionEnv())
            return;
        PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL, out string[] arrStrDefine);
        var listString = arrStrDefine.ToList();
        listString.Add("TEST_PRODUCTION_BUILD");
        if (!listString.Contains("PRODUCTION_BUILD"))
        {
            listString.Add("TEST_PRODUCTION_BUILD");
        }

        var finalString = string.Join(";", listString);
        PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, finalString);
    }

    private static void Build()
    {
        if (IsProduction())
        {
            if (!IsTestProductionEnv())
            {
                var cdn = "https://cdn.mirailabs.co/games/pocketwaifu.io/web-build";
                PlayerSettings.SetTemplateCustomValue("BUILD_URL", $"{cdn}/Build");
                PlayerSettings.SetTemplateCustomValue("STREAMING_ASSETS_URL", $"{cdn}/StreamingAssets");
            }
            else
            {
                PlayerSettings.SetTemplateCustomValue("BUILD_URL", "Build");
                PlayerSettings.SetTemplateCustomValue("STREAMING_ASSETS_URL", "StreamingAssets");
            }
        }
        else
        {
            PlayerSettings.SetTemplateCustomValue("BUILD_URL", "Build");
            PlayerSettings.SetTemplateCustomValue("STREAMING_ASSETS_URL", "StreamingAssets");
        }

        PlayerSettings.SetTemplateCustomValue("TIMESTAMP", "0");
        PlayerSettings.SetTemplateCustomValue("TIMESTAMP", DateTime.Now.Ticks.ToString());

        var scenes = new string[]
        {
            "Assets/_Src/Scenes/Lobby.unity"
        };
        string path = "";
        if (IsProduction())
        {
            if (IsTestProductionEnv())
                path = "Build/production-test";
            else
                path = "Build/production";
        }
        else
        {
            path = "Build/development";
        }

        BuildPipeline.BuildPlayer(scenes, path, BuildTarget.WebGL, BuildOptions.None);
    }

    private static bool IsProduction()
    {
        PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL, out string[] arrStrDefine);
        var listString = arrStrDefine.ToList();
        if (listString.Exists(x => x.Equals("PRODUCTION_BUILD")))
        {
            return true;
        }

        return false;
    }

    private static bool IsTestProductionEnv()
    {
        PlayerSettings.GetScriptingDefineSymbols(NamedBuildTarget.WebGL, out string[] arrStrDefine);
        var listString = arrStrDefine.ToList();
        if (listString.Exists(x => x.Equals("TEST_PRODUCTION_BUILD")))
        {
            return true;
        }

        return false;
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        var files = report.GetFiles();
        var html = "";
        var cssHash = "";

        foreach (var file in files)
        {
            var path = file.ToString();
            var fileName = Path.GetFileName(path);
            if (fileName == "index.html")
            {
                html = File.ReadAllText(file.ToString());
            }
            else if (fileName == "styles.css")
            {
                var md5 = MD5.Create();
                var stream = File.OpenRead(path);
                var hash = md5.ComputeHash(stream);

                cssHash = Math.Abs(BitConverter.ToString(hash).Replace("-", "").GetHashCode()).ToString();
                stream.Close();
            }
        }

        html = html.Replace("styles.css", $"styles.{cssHash}.css");

        File.Copy(Path.Combine(report.summary.outputPath, "styles.css"), Path.Combine(report.summary.outputPath, $"styles.{cssHash}.css"), true);
        File.Delete(Path.Combine(report.summary.outputPath, "styles.css"));
        File.WriteAllText(Path.Combine(report.summary.outputPath, "index.html"), html);
    }

    public static void LogProduction()
    {
        PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.None);
    }
    public static void LogDEV()
    {
        PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
        PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
        PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
        PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
        // UnityEditor.AssetDatabase.SaveAssets();
        // UnityEditor.AssetDatabase.Refresh();
    }
}
