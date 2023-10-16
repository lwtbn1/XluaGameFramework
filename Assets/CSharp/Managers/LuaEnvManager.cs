using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaEnvManager : MonoSingletonBase<LuaEnvManager>
{
    private LuaEnv m_luaEnv;
    private LuaTable m_scriptEnv;
    public void Initialize()
    {
        m_luaEnv = new LuaEnv();
        m_scriptEnv = m_luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_luaEnv.NewTable();
        meta.Set("__index", m_luaEnv.Global);
        m_scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        m_scriptEnv.Set("self", this);
        m_luaEnv.AddLoader((ref string luaFileName) => {
            Debug.Log($"lua file name : {luaFileName}");
            var pathSplits = luaFileName.Split('.');
#if UNITY_EDITOR
            var luaPath = ConstDefine.LUA_SRC_ROOT_PATH;
            for(var ix =0; ix < pathSplits.Length; ix++) {
                luaPath += pathSplits[ix] + ((ix == pathSplits.Length - 1)? ".lua" : "/");
            }
            Debug.Log($"lua path : {luaPath}");
            return FileTools.LoadLuaFileUTF8Bytes(luaPath);
#else
            return null;
#endif

        });
    }
    // Start is called before the first frame update
    void Start()
    {
        m_luaEnv.DoString("require('LuaMain')");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_luaEnv != null)
        {
            m_luaEnv.Tick();
        }
    }

    void OnDestroy()
    {
        m_luaEnv.Dispose();
    }


}
