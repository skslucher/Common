using UnityEngine;
using System.Collections;

[AddComponentMenu("Common/Pooled Object")]
public class PooledObject : MonoBehaviour {

    [HideInInspector]
    public bool isPreWarm = false;

    public bool isCorpse = false;

    private ObjectPool _parentPool;
    public ObjectPool parentPool
    {
        get
        {
            return _parentPool;
        }
        set
        {
            _parentPool = value;
        }
    }

    public bool dirty = false;
}
