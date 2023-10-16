using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileTools
{
    public static byte[] LoadLuaFileUTF8Bytes(string path)
    {
        
        if (!File.Exists(path))
        {
            Debug.LogError($"����lua�ļ�����lua�ļ������ڣ�{path}");
            return null;
        }
        byte[] bytes = null;
        try
        {
            using (var fs = new StreamReader(path))
            {
                var txt = fs.ReadToEnd();
                bytes = System.Text.Encoding.UTF8.GetBytes(txt);
            }
        }catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
        return bytes;
    }
}
