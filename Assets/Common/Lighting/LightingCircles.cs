using UnityEngine;
using System.Collections;

public enum LightingCircleScaling
{
    Linear,
    Inverse
}

public class LightingCircles : MonoBehaviour {

    public LightingCircleScaling scaling = LightingCircleScaling.Inverse;
    
    public float radius;

    public Transform[] circles;

    public float[] offsets;
    public float[] factors;

    public float threshold = 1f;

    public GameObject parent;
    //const float scale = .0125;

    void OnValidate()
    {
        SetRadius(radius);
    }

    void Update()
    {
        if(parent == null || !parent.activeSelf)
        {
            ObjectManager.Destroy(gameObject);
        }
    }

    public void SetRadius(float newRadius)
    {
        radius = Mathf.Max(0f, newRadius);
        Refresh();
    }
    
    void Refresh()
    {
        for(int i=0; i<circles.Length; i++)
        {
            float scale = CircleRadius(i);
            circles[i].localScale = new Vector3(scale, scale, 1f);
        }
    }

    float CircleRadius(int index)
    {
        switch (scaling)
        {
            case LightingCircleScaling.Linear:
                return LinearRadius(index);
            case LightingCircleScaling.Inverse:
                return InverseRadius(index);
        }
        return 0f;
    }

    float LinearRadius(int index)
    {
        float t = factors[index];

        float y = Mathf.Lerp(-threshold, radius, t);
        return Mathf.Max(0f, y);
    }

    float InverseRadius(int index)
    {
        float x = radius - threshold - offsets[index];
        if (x <= 0f)
        {
            return 0f;
        }

        float y = x - (1f / x);
        y += threshold;
        y = Mathf.Max(0f, y);
        return y;
    }
}
