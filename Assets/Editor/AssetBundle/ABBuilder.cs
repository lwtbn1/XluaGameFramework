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
        var outputPath = Application.streamingAssetsPath + "/Pack";
        if(Directory.Exists(outputPath))
            Directory.Delete(outputPath, true);
        Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, litAssetBundle.ToArray(),
            BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
            EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    static List<AssetBundleBuild>  CollectionAssets()
    {
        var litAssetBundle = new List<AssetBundleBuild>();
        var litAbConfig = ABConfigCollector.Datas;
        foreach (var abConfig in litAbConfig)
        {
            var path = AssetDefine.AssetRoot + abConfig.Path;
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
                case EABBuildStrategy.AllFileOneAB:
                {
                    CollectionAssetsRAllFileOneAB(litAssetBundle,path);
                    break;
                }
            }
        }

        return litAssetBundle;
    }

    static void CollectionAssetsEveryFilePerAB(List<AssetBundleBuild> lit,string rootPath)
    {
        var filePaths = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);
        foreach (var filePath in filePaths)
        {
            var fileInfo = new FileInfo(filePath);
            if (LegalAssets.IsLegal(fileInfo))
            {
                var assetNames = new List<string>();
                assetNames.Add(FileTools.GetAssetPathL(filePath));
                var assetBundleBuild = new AssetBundleBuild();
                assetBundleBuild.assetBundleName = FileTools.FullFileNameToABNameL(filePath);
                assetBundleBuild.assetNames = assetNames.ToArray();
                lit.Add(assetBundleBuild);
            }
        }
    }
    static void CollectionAssetsEveryFolderPerAB(List<AssetBundleBuild> lit,string rootPath)
    {
        var dirPaths = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var dirPath in dirPaths)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            var assetNames = new List<string>();
            
            var fileInfos = dirInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (LegalAssets.IsLegal(fileInfo))
                {
                    assetNames.Add(FileTools.GetAssetPathL(fileInfo.FullName));
                }
            }
            if (assetNames.Count > 0)
            {
                var assetBundleBuild = new AssetBundleBuild();
                assetBundleBuild.assetBundleName = FileTools.DirToABNameL(dirInfo.FullName);
                assetBundleBuild.assetNames = assetNames.ToArray();
                lit.Add(assetBundleBuild);
            }
        }
    }
    static void CollectionAssetsRecursionFolderPerAB(List<AssetBundleBuild> lit, string rootPath)
    {
        var dirPaths = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
        foreach (var dirPath in dirPaths)
        {
            var dirInfo = new DirectoryInfo(dirPath);
            var assetNames = new List<string>();
            var fileInfos = dirInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                if (LegalAssets.IsLegal(fileInfo))
                {
                    assetNames.Add(FileTools.GetAssetPathL(fileInfo.FullName));
                }
            }
            if (assetNames.Count > 0)
            {
                var assetBundleBuild = new AssetBundleBuild();
                assetBundleBuild.assetBundleName = FileTools.DirToABNameL(dirInfo.FullName);
                assetBundleBuild.assetNames = assetNames.ToArray();
                lit.Add(assetBundleBuild);
            }
        }
    }

    static void CollectionAssetsRAllFileOneAB(List<AssetBundleBuild> lit, string rootPath)
    {
        var filePaths = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);
        var assetNames = new List<string>();
       
        foreach (var filePath in filePaths)
        {
            var fileInfo = new FileInfo(filePath);
            if (LegalAssets.IsLegal(fileInfo))
            {
                assetNames.Add(FileTools.GetAssetPathL(filePath));
            }
        }

        if (assetNames.Count > 0)
        {
            var assetBundleBuild = new AssetBundleBuild();
            assetBundleBuild.assetBundleName = FileTools.DirToABNameL(rootPath);
            assetBundleBuild.assetNames = assetNames.ToArray();
            lit.Add(assetBundleBuild);
        }
    }
}
