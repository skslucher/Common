using UnityEngine;
using System.Collections;


public class RandUtil : Singleton<RandUtil> {
    
    static public Vector3 insideUnitCube //i.e. square radius of 1: (-1,-1,-1) to (1,1,1)
    {
        get
        {
            return new Vector3(variance, variance, variance);
        }
    }
    static public Vector2 insideUnitSquare
    {
        get
        {
            return new Vector2(variance, variance);
        }
    }

    static public float variance
    {
        get
        {
            return -1f + Random.value * 2f;
        }
    }


    static public float Variance(float magnitude)
    {
        return variance * magnitude;
    }

    static public float Variance(float center, float magnitude)
    {
        return center + variance * magnitude;
    }


    static public int Int(float top)
    {
        return Mathf.FloorToInt(Random.value * top);
    }



    [System.Serializable]
    public class WeightedObject
    {
        public GameObject obj;
        public float weight = 1.0f;
    }

    [System.Serializable]
    public class WeightedInt
    {
        public int index;
        public float weight = 1.0f;
    }

    static public GameObject WeightedObj(WeightedObject[] objects)
    {
        float tot = 0f;
        foreach (WeightedObject obj in objects)
        {
            tot += obj.weight;
        }
        tot *= Random.value;

        foreach (WeightedObject obj in objects)
        {
            tot -= obj.weight;

            if (tot < 0)
            {
                return obj.obj;
            }
        }
        return null;
    }

}
