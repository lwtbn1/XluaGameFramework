using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstDefine
{
    
#if UNITY_ANDROID
    public static string LUA_SRC_ROOT_PATH = $"{Application.streamingAssetsPath}/SrcLua/";
#else
    public static string LUA_SRC_ROOT_PATH = $"{Application.dataPath}/../SrcLua/";
#endif

}
