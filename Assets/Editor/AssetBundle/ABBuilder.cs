using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ABBuilder
{
    [MenuItem("ABTools/Build")]
    public static void Build()
    {
        ABConfigCollector.Load();
        
        var litAssetBundle = CollectionAssets();
        //AssetBundleBuild
        var outputPath = Application.persistentDataPath + "/Pack";
        BuildPipeline.BuildAssetBundles(outputPath, litAssetBundle.ToArray(),
            BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            EditorUserBuildSettings.activeBuildTarget);
    }

    static List<AssetBundleBuild>  CollectionAssets()
    {
        var litAssetBundle = new List<AssetBundleBuild>();
        var litAbConfig = ABConfigCollector.Datas;
        foreach (var abConfig in litAbConfig)
        {
            var path = abConfig.Path;
            var strategy = abConfig.Strategy;
        }

        return litAssetBundle;
    }
}
