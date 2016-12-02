using UnityEngine;
using UnityEditor;
using System.Collections;

public class TextureImportSettings  {
    

    [MenuItem("CONTEXT/TextureImporter/Auto Pixel Import")]
    public static void AutoPixelImport()
    {
        if (Selection.activeObject.GetType() != typeof(Texture2D))
        {
            Debug.Log("Not a texture");
            return;
        }

        Texture2D texture = Selection.activeObject as Texture2D;


        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        textureImporter.spritePixelsPerUnit = 16f;
        textureImporter.mipmapEnabled = false;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.alphaIsTransparency = true;

        EditorUtility.SetDirty(textureImporter);
        textureImporter.SaveAndReimport();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }
}
