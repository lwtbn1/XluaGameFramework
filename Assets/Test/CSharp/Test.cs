using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FileInfoExtension();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FileInfoExtension()
    {
        var files = Directory.GetFiles(Application.dataPath + "\\Datas\\Common\\Texture", "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            Debug.Log(fileInfo.FullName);
            Debug.Log(fileInfo.Extension);
            Debug.Log(fileInfo.Name);
            Debug.Log("===================");
        }
    }
}
