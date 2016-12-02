using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateTextureFromPalette : MonoBehaviour {

    public Texture2D paletteTexture;

    public enum TextureSource { Texture, Algorithm }
    public TextureSource textureSource = TextureSource.Texture;

    public Texture2D defaultLUT;
    public int dimensions = 16;

    public string filepath = "New LUT";

    [ContextMenu("Generate Texture")]
    public void GenerateTexture()
    {
        Color[] palette = GetColorsFrom(paletteTexture);

        Color[] colors;
        switch (textureSource)
        {
            case TextureSource.Texture:
                colors = defaultLUT.GetPixels();
                dimensions = defaultLUT.height;
                break;
            case TextureSource.Algorithm:
                colors = GenerateLUT(dimensions);
                break;
            default:
                return;
        }
        
        colors = LimitColorsTo(colors, palette);
        

        Texture2D newTexture = new Texture2D(dimensions * dimensions, dimensions);
        newTexture.SetPixels(colors);

        SaveFile.Save(filepath, newTexture);
    }


    public static Color[] GetColorsFrom(Texture2D paletteTexture)
    {
        List<Color> palette = new List<Color>();

        Color[] texColors = paletteTexture.GetPixels();

        for(int i=0; i<texColors.Length; i++)
        {
            if (!palette.Contains(texColors[i])) palette.Add(texColors[i]);
        }

        return palette.ToArray();
    }

    public static Color[] GenerateLUT(int dim)
    {
        Color[] colors = new Color[dim * dim * dim];
        float interval = 1f / dim;

        for(int i=0; i< dim; i++)
        {
            for(int j=0; j< dim; j++)
            {
                for(int k=0; k< dim; k++)
                {
                    colors[i + dim * (k + dim * j)] = new Color(i * interval, (dim - j) * interval, k * interval);
                }
            }
        }

        return colors;
    }


    public static Color[] LimitColorsTo(Color[] colors, Color[] palette)
    {
        for(int i=0; i<colors.Length; i++)
        {
            Color newColor = palette[0];

            for(int j=1; j<palette.Length; j++)
            {
                if (DistanceRaw(colors[i], palette[j]) < DistanceRaw(colors[i], newColor)) newColor = palette[j];
            }

            colors[i] = newColor;
        }

        return colors;
    }


    public static float DistanceRaw(Color color1, Color color2)
    {
        return Mathf.Abs(color1.r - color2.r) + Mathf.Abs(color1.g - color2.g) + Mathf.Abs(color1.b - color2.b);
    }

    public static float Distance(Color color1, Color color2)
    {
        return Mathf.Sqrt(Com.Pow(color1.r - color2.r, 2) + Com.Pow(color1.g - color2.g, 2) + Com.Pow(color1.b - color2.b, 2));
    }

}
