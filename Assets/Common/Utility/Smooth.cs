using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Smooth
{
    public enum LerpMod
    {
        SmoothStep,
        SmootherStep,
        FadeIn,
        FadeOut
    }

    public delegate float FactorFunc(float t);
    
    static public FactorFunc[] factorFuncs;

    
    static Smooth()
    {
        factorFuncs = new FactorFunc[4];

        factorFuncs[0] = SmoothStep;
        factorFuncs[1] = SmootherStep;
        factorFuncs[2] = FadeIn;
        factorFuncs[3] = FadeOut;
    }
    


    static public float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }
    static public float SmootherStep(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }
    static public float FadeIn(float t)
    {
        return t * t;
    }
    static public float FadeOut(float t)
    {
        //return 1f - (1f - t) * (1f - t);
        return t * (2f - t);
    }

    

    static public float Lerp(float a, float b, float t, LerpMod lerpMod = LerpMod.SmoothStep)
    {
        if (t <= 0f) return a;
        if (t >= 1f) return b;

        t = factorFuncs[(int)lerpMod](t);

        return a + t * (b - a);
    }

    static public float LerpUnclamped(float a, float b, float t, LerpMod lerpMod = LerpMod.SmoothStep)
    {
        t = factorFuncs[(int)lerpMod](t);

        return a + t * (b - a);
    }

    static public float Factor(float t, LerpMod lerpMod = LerpMod.SmoothStep)
    {
        return factorFuncs[(int)lerpMod](t);
    }



    const float defaultBuffer = .5f;


    static public float Min(float t, float min, float buffer = defaultBuffer)
    {
        if (t > min + buffer) return t;
        if (t < min - buffer) return min;
        
        return MinUnclamped(t, min, buffer);
    }

    static private float MinUnclamped(float t, float min, float buffer = defaultBuffer)
    {
        return LerpUnclamped(min + buffer, min, Mathf.InverseLerp(min + buffer, min - buffer, t), LerpMod.FadeOut);
    }


    static public float Max(float t, float max, float buffer = defaultBuffer)
    {
        if (t < max - buffer) return t;
        if (t > max + buffer) return max;
        
        return MaxUnclamped(t, max, buffer);
    }

    static private float MaxUnclamped(float t, float max, float buffer = defaultBuffer)
    {
        return LerpUnclamped(max - buffer, max, Mathf.InverseLerp(max - buffer, max + buffer, t), LerpMod.FadeOut);
    }


    static public float Clamp(float t, float min, float max, float buffer = defaultBuffer)
    {
        if (min >= max) return min;
        
        if (buffer < 0) buffer = 0f;
        else if (buffer > .5f * (max - min)) buffer = .5f * (max - min);

        if (t < min - buffer) return min;
        else if (t < min + buffer) return MinUnclamped(t, min, buffer);
        else if (t < max - buffer) return t;
        else if (t < max + buffer) return MaxUnclamped(t, max, buffer);
        else return max;
    }
    static public float ClampAuto(float t, float clamp1, float clamp2, float buffer = defaultBuffer)
    {
        if (clamp1 == clamp2) return clamp1;

        if (clamp1 < clamp2) return Clamp(t, clamp1, clamp2, buffer);
        else return Clamp(t, clamp2, clamp1, buffer);
    }

    
    
}
