using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LegalAssets
{
    
    
    public static List<string> prefix = new List<string>()
    {
        ".png",".prefab"
    };

    public static bool IsLegal(FileInfo fileInfo)
    {
        return prefix.Contains(fileInfo.Extension); 
    }
}

public static class AssetDefine
{
    public const string ASSET_PREFIX = "Datas";
}
