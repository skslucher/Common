using UnityEngine;
using System.Collections;



public class Com : MonoBehaviour {


    const string twitterURL = "https://twitter.com/skslucher";
    public static void OpenTwitter()
    {
#if UNITY_WEBGL
        Application.ExternalEval("window.open(\"" + twitterURL + "\")");
#else
        Application.OpenURL(twitterURL);
#endif
    }
    

    static public float Pow(float value, int power)
    {
        if (power == 0) return 1f;

        if (power < 0)
        {
            value = 1f / value;
            power = -power;
        }

        float returnValue = value;
        for (int i = 1; i < power; i++)
        {
            returnValue *= value;
        }
        return returnValue;
    }


    static public bool BetweenExclusive(float value, float min, float max)
    {
		return (value > min && value < max);
	}
    static public bool BetweenInclusive(float value, float min, float max)
    {
        return (value >= min && value <= max);
    }


    

	static public bool NonAlpha(Color color1, Color color2){
		return (color1.r == color2.r && color1.g == color2.g && color1.b == color2.b);
	}

    static public float Normalize(float value)
    {
        if (value > 0f) return 1f;
        if (value < 0f) return -1f;
        return 0f;
    }
    static public int Normalize(int value)
    {
        if (value > 0) return 1;
        if (value < 0) return -1;
        return 0;
    }

    static public Rect Combine(Rect rect1, Rect rect2)
    {
        Rect rect = new Rect();

        rect.xMin = Mathf.Min(rect1.xMin, rect2.xMin);
        rect.xMax = Mathf.Max(rect1.xMax, rect2.xMax);
        rect.yMin = Mathf.Min(rect1.yMin, rect2.yMin);
        rect.yMax = Mathf.Max(rect1.yMax, rect2.yMax);

        return rect;
    }


    static public Rect[] SplitRect(Rect original, int num, float buffer = 0f, bool xAxis = true)
    {
        Rect[] split = new Rect[num];

        Rect blueprint = original;
        Vector2 offset = Vector2.zero;

        if (num > 1)
        {
            if (xAxis)
            {
                float length = original.width;
                blueprint.width = length / num - buffer / (num - 1);
                offset.x = (length + buffer) / num;
            }
            else
            {
                float length = original.height;
                blueprint.height = length / num - buffer / (num - 1);
                offset.y = (length + buffer) / num;
            }
        }

        for (int i=0; i< num; i++)
        {
            split[i] = blueprint;
            blueprint.position += offset;
        }


        return split;
    }


    public static Vector3 Round(Vector3 vector, float interval = 1f)
    {
        if (interval == 0f) return vector;

        vector /= interval;

        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);
        vector.z = Mathf.Round(vector.z);

        vector *= interval;

        return vector;
    }

    public static Vector2 Round(Vector2 vector, float interval = 1f)
    {
        if (interval == 0f) return vector;

        vector /= interval;

        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);

        vector *= interval;

        return vector;
    }

    public static float Round(float number, float interval = 1f)
    {
        if (interval == 0f) return number;

        number /= interval;

        number = Mathf.Round(number);

        number *= interval;

        return number;
    }


    public static int BoolToInt(bool value)
    {
        if (value) return 1;
        else return 0;
    }

    public static bool IntToBool(int value)
    {
        return (value == 1);
    }

    public enum Axis { XAxis, YAxis, ZAxis }
    public static float GetAxis(Vector3 vector, Axis axis)
    {
        switch (axis)
        {
            case Axis.XAxis:
                return vector.x;
            case Axis.YAxis:
                return vector.y;
            case Axis.ZAxis:
                return vector.z;
        }
        return 0f;
    }
    public static Vector3 SetAxis(Vector3 vector, Axis axis, float value)
    {
        switch (axis)
        {
            case Axis.XAxis:
                vector.x = value;
                break;
            case Axis.YAxis:
                vector.y = value;
                break;
            case Axis.ZAxis:
                vector.z = value;
                break;
        }
        return vector;
    }


    static public float CorrectLerpFactor(float lerpFactor)
    {
        return 1f - Mathf.Pow(1f - lerpFactor, Time.deltaTime);
    }

    static public float CorrectLerpFactor(float lerpFactor, float deltaTime)
    {
        return 1f - Mathf.Pow(1f - lerpFactor, deltaTime);
    }


    static public float ClampAuto(float value, float clamp1, float clamp2)
    {
        if (clamp1 > clamp2) return Mathf.Clamp(value, clamp2, clamp1);
        else return Mathf.Clamp(value, clamp1, clamp2);
    }


    static public float SmoothLerpIn(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, SmoothIn(t));
    }
    static public float SmoothIn(float t)
    {
        return 1f - Mathf.Cos(t * Mathf.PI * .5f);
    }

    static public float SmoothLerpOut(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, SmoothOut(t));
    }
    static public float SmoothOut(float t)
    {
        return Mathf.Sin(t * .5f * Mathf.PI);
    }

    static public float SmoothLerpBoth(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, SmoothBoth(t));
    }
    static public float SmoothBoth(float t)
    {
        return .5f * (1 + Mathf.Cos((1 + t) * Mathf.PI));
    }


    static public GameObject FindOrInstantiateWithTag(string tag)
    {
        GameObject returnObject = GameObject.FindWithTag(tag);
        if(returnObject == null)
        {
            returnObject = new GameObject();
            returnObject.tag = tag;
            returnObject.name = tag;
            returnObject.transform.Zero();
        }
        return returnObject;
    }


    ////

}
