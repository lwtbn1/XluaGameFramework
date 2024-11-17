using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;
using XLua;

[LuaCallCSharp]
public static class FileTools
{
    public static byte[] LoadLuaFileUTF8Bytes(string path)
    {
        
        byte[] bytes = null;
        try
        {
#if UNITY_EDITOR
            bytes = File.ReadAllBytes(path);

#else
#if UNITY_ANDROID
            WWW www = new WWW(path);
            while (!www.isDone) { }
            bytes = www.bytes;
#endif
#endif

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return bytes;
    }

    public static string GetAssetPath(string fullPath)
    {
        var legalFullPath = fullPath.Replace('\\', '/');
        var startIx = legalFullPath.IndexOf(Application.dataPath, StringComparison.Ordinal | StringComparison.OrdinalIgnoreCase);
        return legalFullPath.Substring(startIx);
    }
    public static void Test(int i)
    {

    }

    public static void Test1(GameObject gameObject)
    {

    }
}
