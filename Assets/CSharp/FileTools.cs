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

    public static string GetAssetPathL(string fullPath)
    {
        var legalFullPath = fullPath.Replace('\\', '/').ToLower();
        var startIx = Application.dataPath.Length + 1;
        return legalFullPath.Substring(startIx - 7);
    }

    public static string DirToABNameL(string fullPath)
    {
        var legalFullPath = fullPath.Replace('\\', '/').ToLower();
        var startix = AssetDefine.AssetRoot.Length;
        return legalFullPath.Substring(startix).Replace('/', '#') + ".ab";
    }

    public static string FullFileNameToABNameL(string fullFileName)
    {
        var noExtensionFileName = fullFileName.Replace(Path.GetExtension(fullFileName),"");
        var legalFullPath = noExtensionFileName.Replace('\\', '/').ToLower();
        var startix = AssetDefine.AssetRoot.Length;
        return legalFullPath.Substring(startix).Replace('/', '#') + ".ab";
    }
    public static void Test(int i)
    {

    }

    public static void Test1(GameObject gameObject)
    {

    }
}
