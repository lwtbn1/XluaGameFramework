using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EABBuildStrategy
{
    /// <summary>
    /// 当前目录下每个目录打一个AB
    /// </summary>
    EveryFolderPerAB,
    /// <summary>
    /// 当前目录下每个文件打一个AB
    /// </summary>
    EveryFilePerAB,
    /// <summary>
    /// 当前目录下递归寻找，每个目录打一个AB
    /// </summary>
    RecursionFolderPerAB,
}

public class ABConfig
{
    /// <summary>
    /// 路径,从Datas路径下开始算
    /// </summary>
    public string Path;
    /// <summary>
    /// 打包策略
    /// </summary>
    public EABBuildStrategy Strategy;
    /// <summary>
    /// 是否是通用资源，通用资源不卸载AB
    /// </summary>
    public bool IsCommon;
}

public class ABConfigCollector
{
    public static List<ABConfig> Datas;

    public static void Load()
    {
        var textAsset = Resources.Load<TextAsset>("Assetbundle/ABConfig");
        var jsonStr = textAsset.text;
        Datas = LitJson.JsonMapper.ToObject<List<ABConfig>>(jsonStr);
    }

}
