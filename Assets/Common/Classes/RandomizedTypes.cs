using UnityEngine;
using System.Collections;



public enum NumberScale
{
    Linear,
    Logarithmic
}

public enum RandomizedFloatType
{
    Constant,
    MinMax,
    Percent,
    Variance
}


[System.Serializable]
public class RandomizedFloat
{

    public static implicit operator float(RandomizedFloat f)
    {
        return f.Get();
    }

    public static implicit operator RandomizedFloat(float f)
    {
        return new RandomizedFloat(f);
    }

    public RandomizedFloat(float f)
    {
        variableType = RandomizedFloatType.Constant;
        baseValue = f;
    }

    public RandomizedFloatType variableType = RandomizedFloatType.Constant;

    public NumberScale numberScale = NumberScale.Linear;

    public float baseValue = 1f;
    public float modifier = 0f;

    public float Get()
    {
        switch (variableType)
        {
            case RandomizedFloatType.Constant:
                return Constant();
            case RandomizedFloatType.MinMax:
                return MinMax();
            case RandomizedFloatType.Percent:
                return Percent();
            case RandomizedFloatType.Variance:
                return Variance();
        }
        return 0f;
    }

    public float Constant()
    {
        return baseValue;
    }

    public float MinMax()
    {
        return Mathf.Lerp(baseValue, modifier, Random.value);
    }

    public float Percent()
    {
        if (modifier == 0f)
        {
            return Constant();
        }
        else
        {
            if (numberScale == NumberScale.Linear)
            {
                float percentModifier = 1f - modifier;
                if (Random.value > .5f)
                {
                    percentModifier = 1f / percentModifier;
                }
                return baseValue * Mathf.Lerp(1f, percentModifier, Random.value);
            }
            else
            {
                float variance = Mathf.Log(1f - modifier, 2f);
                variance = Mathf.Lerp(-variance, variance, Random.value);
                variance = Mathf.Pow(2f, variance);
                return baseValue * variance;
            }
        }
    }

    public float Variance()
    {
        return baseValue + modifier * Mathf.Lerp(-1f, 1f, Random.value);
    }

    public float Center()
    {
        switch (variableType)
        {
            case RandomizedFloatType.Constant:
                return Constant();
            case RandomizedFloatType.MinMax:
                return Mathf.Lerp(baseValue, modifier, .5f);
            case RandomizedFloatType.Percent:
                return Constant();
        }
        return 0f;
    }
}

public enum ReferenceFrame
{
    Relative,
    Absolute
}

public enum RandomizedVectorType
{
    Constant,
    Distance,
    Range,
    Bounded
}

[System.Serializable]
public class RandomizedVector3
{

    public static implicit operator Vector3(RandomizedVector3 vector)
    {
        return vector.Get();
    }

    public ReferenceFrame referenceFrame = ReferenceFrame.Relative;
    public RandomizedVectorType variableType = RandomizedVectorType.Constant;

    public Vector3 center = Vector3.zero;

    public float distance = 0f;
    public Vector3 bound1 = Vector3.zero;
    public Vector3 bound2 = Vector3.zero;

    public Vector3 Get()
    {
        return Center() + Offset();
    }

    public Vector3 Center()
    {
        switch (referenceFrame)
        {
            case ReferenceFrame.Relative:
                return Vector3.zero;
            case ReferenceFrame.Absolute:
                return center;
        }
        return Vector3.zero;
    }

    public Vector3 Offset()
    {
        switch (variableType)
        {
            case RandomizedVectorType.Constant:
                return Vector3.zero;

            case RandomizedVectorType.Distance:
                return Random.insideUnitSphere * distance;

            case RandomizedVectorType.Range:
                return Vector3.Scale(bound1, RandUtil.insideUnitCube);

            case RandomizedVectorType.Bounded:
                return new Vector3(
                    Mathf.Lerp(bound1.x, bound2.x, Random.value),
                    Mathf.Lerp(bound1.y, bound2.y, Random.value),
                    Mathf.Lerp(bound1.z, bound2.z, Random.value)
                    );
        }
        return Vector3.zero;
    }

}