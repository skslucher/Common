using UnityEngine;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif


// Based on SavWav.cs Copyright(c) 2012 Calvin Rien

public class SaveFile : MonoBehaviour {

    public static void Save(string filepath, Texture2D texture)
    {
        Save(filepath + ".png", texture.EncodeToPNG());
    }

    public static void Save(string filepath, byte[] bytes)
    {
        //var filepath = Path.Combine(Application.persistentDataPath, filename);
        filepath = Path.Combine(Application.dataPath, filepath);

        Debug.Log(filepath);

        // Make sure directory exists if user is saving to sub dir.
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        using (var fileStream = CreateEmpty(filepath))
        {
            fileStream.Write(bytes, 0, bytes.Length);
        }
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    static FileStream CreateEmpty(string filepath)
    {
        var fileStream = new FileStream(filepath, FileMode.Create);
        
        return fileStream;
    }

}
