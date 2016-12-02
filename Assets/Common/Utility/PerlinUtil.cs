using UnityEngine;
using System.Collections;

public class PerlinUtil {
    
    private static Perlin perlin;
    public static Perlin Perlin
    {
        get
        {
            if(perlin == null)
            {
                perlin = new Perlin();
            }
            return perlin;
        }
    }


    //Correction factors          //Numbers reached through testing
    const float factor1 = 1.144f; //1.146182f;
    const float offset1 = 0.015f; //0.01495817f;
    const float factor2 = 0.860f; //0.8622499f;
    const float offset2 = 0.000f; //-0.0001594424f;

    const float dimensionOffset = 1000f;



    // "Perlin Noise": -0.5 to 0.5, (approximately, at least. if accuracy is important, you should tweak the correction factors)

    static public float Noise(float t)
    {
        return Perlin.Noise(t) * factor1 + offset1;
    }
    static public float Noise(float x, float y)
    {
        return Perlin.Noise(x, y) * factor2 + offset2;
    }

    static public Vector2 Noise2D(float t)
    {
        return new Vector2(Noise(t), Noise(t + dimensionOffset));
    }
    static public Vector2 Noise2D(float x, float y)
    {
        return new Vector2(Noise(x), Noise(y));
    }


    // "Perlin Normalized": 0 to 1

    static public float Normalized(float t)
    {
        return Noise(t) + .5f;
    }
    static public float Normalized(float x, float y)
    {
        return Noise(x, y) + .5f;
    }

    static public Vector2 Normalized2D(float t)
    {
        return Noise2D(t) + Vector2.one * .5f;
    }
    static public Vector2 Normalized2D(float x, float y)
    {
        return Noise2D(x, y) + Vector2.one * .5f;
    }


    // "Perlin Variance": -1 to 1

    static public float Variance(float t)
    {
        return Noise(t) * 2f;
    }
    static public float Variance(float x, float y)
    {
        return Noise(x, y) * 2f;
    }
    
    static public Vector2 Variance2D(float t)
    {
        return Noise2D(t) * 2f;
    }
    static public Vector2 Variance2D(float x, float y)
    {
        return Noise2D(x, y) * 2f;
    }


}
