using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LegalAssets
{
    
    
    public static List<string> prefix = new List<string>()
    {
        ".png",".prefab",".asset",".anim",".controller",".ogg",".mp3"
    };

    public static bool IsLegal(FileInfo fileInfo)
    {
        return prefix.Contains(fileInfo.Extension); 
    }
}

public static class AssetDefine
{
    private static string assetRoot = null;

    public static string AssetRoot
    {
        get
        {
            if(assetRoot == null)
                assetRoot = (Application.dataPath + "/Datas/").ToLower();
            return assetRoot;
        }
    }
}
