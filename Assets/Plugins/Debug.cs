// 
// Copyright (c) 2023 Kim Hyun Deok
//
// Debug.cs
// 
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class Debug 
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    [UnityEditor.Callbacks.OnOpenAsset()]
    private static bool OnOpenDebugLog(int instance, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instance).name;
        if (!name.Equals("Debug")) return false;

        // 에디터 콘솔 윈도우의 인스턴스를 찾는다.
        var assembly = Assembly.GetAssembly(typeof(EditorWindow));
        if (assembly == null) return false;

        var consoleWindowType = assembly.GetType("UnityEditor.ConsoleWindow");
        if (consoleWindowType == null) return false;

        var consoleWindowField = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
        if (consoleWindowField == null) return false;

        var consoleWindowInstance = consoleWindowField.GetValue(null);
        if (consoleWindowInstance == null) return false;

        if (consoleWindowInstance != (object)EditorWindow.focusedWindow) return false;

        // 콘솔 윈도우 인스턴스의 활성화된 텍스트를 찾는다.
        var activeTextField = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
        if (activeTextField == null) return false;

        string activeTextValue = activeTextField.GetValue(consoleWindowInstance).ToString();
        if (string.IsNullOrEmpty(activeTextValue)) return false;

        // 디버그 로그를 호출한 파일 경로를 찾아 편집기로 연다.
        Match match = Regex.Match(activeTextValue, @"\(at (.+)\)");
        if (match.Success) match = match.NextMatch(); // stack trace의 첫번째를 건너뛴다.

        if (match.Success)
        {
            string path = match.Groups[1].Value;
            var split = path.Split(':');
            string filePath = split[0];
            int lineNum = System.Convert.ToInt32(split[1]);

            string dataPath = UnityEngine.Application.dataPath.Substring(0, UnityEngine.Application.dataPath.LastIndexOf("Assets"));
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(dataPath + filePath, lineNum);
            return true;
        }
        return false;
    }
}
