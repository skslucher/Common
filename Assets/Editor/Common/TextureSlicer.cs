using UnityEngine;
using UnityEditor;
using System.Collections;

public class TextureSlicer {

    const int pixelsPerUnit = 16;
    
    [MenuItem("CONTEXT/TextureImporter/Slice As Menu Region Skin")]
    public static void SliceTextureAsMenuRegionSkin()
    {
        if(Selection.activeObject.GetType() != typeof(Texture2D))
        {
            Debug.Log("Texture Slicer only applies to Texture2D");
            return;
        }
        Texture2D texture = Selection.activeObject as Texture2D;

        SpriteMetaData[] spriteMetaData = MenuRegionSkinMetaData(new Vector2(texture.width, texture.height));
        if (spriteMetaData != null)
        {
            ApplyToTexture(texture, spriteMetaData);
        }
    }

    [MenuItem("CONTEXT/TextureImporter/Slice As 32px Spritesheet")]
    public static void SliceTextureAsSpritesheet32()
    {
        SliceTextureAsSpritesheet(32);
    }
    [MenuItem("CONTEXT/TextureImporter/Slice As 16px Spritesheet")]
    public static void SliceTextureAsSpritesheet16()
    {
        SliceTextureAsSpritesheet(16);
    }
    [MenuItem("CONTEXT/TextureImporter/Slice As 8px Spritesheet")]
    public static void SliceTextureAsSpritesheet8()
    {
        SliceTextureAsSpritesheet(8);
    }

    public static void SliceTextureAsSpritesheet(int spriteWidth)
    {
        if (Selection.activeObject.GetType() != typeof(Texture2D))
        {
            Debug.Log("Texture Slicer only applies to Texture2D");
            return;
        }

        Texture2D texture = Selection.activeObject as Texture2D;

        SpriteMetaData[] spriteMetaData = SpritesheetMetaData(new Vector2(texture.width, texture.height), spriteWidth);
        if (spriteMetaData != null)
        {
            ApplyToTexture(texture, spriteMetaData);
        }
    }
    


    public static SpriteMetaData[] MenuRegionSkinMetaData(Vector2 textureDimensions)
    {
        int width = (int)textureDimensions.x;
        int height = (int)textureDimensions.y;

        SpriteMetaData[] spriteMetaData;
        
        spriteMetaData = new SpriteMetaData[18];


        int widthFactor = width / 16;
        int heightFactor = height / 8;
        
        if (widthFactor * 16 != width)
        {
            Debug.Log("Error: Texture width must be a multiple of 16");
            return null;
        }
        if (heightFactor * 8 != height)
        {
            Debug.Log("Error: Texture height must be a multiple of 8");
            return null;
        }


        int[] textureSplit = { 3, 2, 3 };

        int iOffset = 0;
        for (int i = 0; i < 3; i++)
        {
            int jOffset = 0;
            for (int j = 0; j < 3; j++)
            {
                Rect rect = new Rect(widthFactor * iOffset, heightFactor * (8 - jOffset - textureSplit[j]), widthFactor * textureSplit[i], heightFactor * textureSplit[j]);
                
                spriteMetaData[i + 3 * j].rect = rect;
                spriteMetaData[i + 3 * j].pivot = Vector2.one * .5f;
                spriteMetaData[i + 3 * j].name = "Face (" + (i - 1) + ", " + (1 - j) + ")";

                if (spriteMetaData.Length > 9)
                {
                    rect.x += 8 * widthFactor;
                    
                    spriteMetaData[9 + i + 3 * j].rect = rect;
                    spriteMetaData[9 + i + 3 * j].pivot = Vector2.one * .5f;
                    spriteMetaData[9 + i + 3 * j].name = "Base (" + (i - 1) + ", " + (1 - j) + ")";
                }

                jOffset += textureSplit[j];
            }

            iOffset += textureSplit[i];
        }

        return spriteMetaData;
    }


    public static SpriteMetaData[] SpritesheetMetaData(Vector2 textureDimensions, int spriteWidth)
    {
        int width = Mathf.FloorToInt(textureDimensions.x / spriteWidth);
        int height = Mathf.FloorToInt(textureDimensions.y / spriteWidth);

        SpriteMetaData[] spriteMetaData = new SpriteMetaData[width * height];
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Rect rect = new Rect(i * spriteWidth, (height - 1 - j) * spriteWidth, spriteWidth, spriteWidth);

                spriteMetaData[i + width * j].rect = rect;
                spriteMetaData[i + width * j].pivot = Vector2.one * .5f;
                spriteMetaData[i + width * j].name = "(" + i + ", " + j + ")";
            }
        }

        return spriteMetaData;
    }
    

    public static void ApplyToTexture(Texture2D texture, SpriteMetaData[] spriteMetaData)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        textureImporter.textureType = TextureImporterType.Advanced;
        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        textureImporter.spritePixelsPerUnit = pixelsPerUnit;
        textureImporter.mipmapEnabled = false;

        textureImporter.spritesheet = spriteMetaData;
        EditorUtility.SetDirty(textureImporter);
        textureImporter.SaveAndReimport();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }

}
