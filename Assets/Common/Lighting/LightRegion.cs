using UnityEngine;
using System.Collections;

public class LightRegion : MonoBehaviour {

    public int lightLevel;
    public Rect bounds;

    public GameObject region;
    
	void OnEnable () {
        region = ObjectManager.Instantiate("Light Region", transform.TransformPoint(bounds.position));
        region.transform.localScale = (Vector3)bounds.size + Vector3.forward;
        region.transform.rotation = transform.rotation;

        foreach(Transform child in region.transform)
        {
            child.gameObject.SetActive(child.gameObject.name == lightLevel.ToString("0"));
        }

        //GetComponent<MeshRenderer>().material = materials[lightLevel];
	}

    void Update()
    {
        region.transform.localScale = (Vector3)(bounds.size + Random.insideUnitCircle*.25f) + Vector3.forward ;
    }
}
