using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            switch (strategy)
            {
                case EABBuildStrategy.EveryFilePerAB:
                {
                    CollectionAssetsEveryFilePerAB(litAssetBundle,path);
                    break;
                }
                case EABBuildStrategy.EveryFolderPerAB:
                {
                    CollectionAssetsEveryFolderPerAB(litAssetBundle,path);
                    break;
                }
                case EABBuildStrategy.RecursionFolderPerAB:
                {
                    CollectionAssetsRecursionFolderPerAB(litAssetBundle,path);
                    break;
                }
            }
        }

        return litAssetBundle;
    }

    static void CollectionAssetsEveryFilePerAB(List<AssetBundleBuild> lit,string rootPath)
    {
        var filePaths = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);
        var assetBundleName = rootPath.ToLower();
        var assetNames = new List<string>();
        var assetBundleBuild = new AssetBundleBuild();
        assetBundleBuild.assetBundleName = assetBundleName;
        foreach (var filePath in filePaths)
        {
            var fileInfo = new FileInfo(filePath);
            if (LegalAssets.IsLegal(fileInfo))
            {
                assetNames.Add(FileTools.GetAssetPath(filePath).ToLower());
            }
        }

        assetBundleBuild.assetNames = assetNames.ToArray();
        lit.Add(assetBundleBuild);
    }
    static void CollectionAssetsEveryFolderPerAB(List<AssetBundleBuild> lit,string rootPath)
    {
        var dirPaths = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var dirPath in dirPaths)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            var assetNames = new List<string>();
            var assetBundleBuild = new AssetBundleBuild();
            assetBundleBuild.assetBundleName = FileTools.GetAssetPath(dirInfo.FullName).ToLower();
            var fileInfos = dirInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (LegalAssets.IsLegal(fileInfo))
                {
                    assetNames.Add(FileTools.GetAssetPath(fileInfo.FullName).ToLower());
                }
            }
            assetBundleBuild.assetNames = assetNames.ToArray();
            
            lit.Add(assetBundleBuild);
        }
    }
    static void CollectionAssetsRecursionFolderPerAB(List<AssetBundleBuild> lit, string rootPath)
    {
        var dirPaths = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
        
        foreach (var dirPath in dirPaths)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            var assetNames = new List<string>();
            var assetBundleBuild = new AssetBundleBuild();
            assetBundleBuild.assetBundleName = FileTools.GetAssetPath(dirInfo.FullName).ToLower();
            var fileInfos = dirInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (LegalAssets.IsLegal(fileInfo))
                {
                    assetNames.Add(FileTools.GetAssetPath(fileInfo.FullName).ToLower());
                }
            }
            assetBundleBuild.assetNames = assetNames.ToArray();
            
            lit.Add(assetBundleBuild);
        }
    }
}
