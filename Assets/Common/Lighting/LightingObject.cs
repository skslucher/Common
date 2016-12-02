using UnityEngine;
using System.Collections;

public class LightingObject : MonoBehaviour {

    public float radius = 100f;
    const float scale = 1f / 8f;
    //public float[] radii;

    public float percentVar = .01f;
    float var = 1f;

    Perlin perlin;
    float seed;

    LightingCircles circles;

    void Awake()
    {
        perlin = new Perlin();
        seed = 100f * Random.value;
    }

    void OnEnable()
    {
        circles = ObjectManager.Instantiate("Light Circles", transform.position).GetComponent<LightingCircles>();
        circles.parent = gameObject;
        //circles.transform.parent = transform;

        circles.SetRadius(scale * radius);
    }


    void Start()
    {
        //Global.instance.gameObject.GetComponent<LightingHandler>().AddLight(this);
        //Global.instance.gameObject.GetComponent<LightingMaterialHandler>().AddLight(this);



        //circles.SetRadius(scale * radius);
    }

    void Update()
    {
        circles.transform.position = transform.position;

        var = (1 + percentVar * perlin.Noise(seed + Time.time * 10f));
        circles.SetRadius(scale * radius * var);
    }

    public int Radius(int tier)
    {
        return Mathf.RoundToInt(radius * var);
        /*
        if (tier < radii.Length)
        {
            return Mathf.RoundToInt(radii[tier] * var);
        }
        else
        {
            return 0;
        }*/
    }

    void OnDisable()
    {
        //Destroy(circles);
        //if(circles != null && circles.gameObject.activeSelf)
        //    ObjectManager.Destroy(circles.gameObject);
    }

}
