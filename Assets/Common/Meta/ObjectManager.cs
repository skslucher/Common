using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*public enum ObjectLabel
{
    Unlabeled,

    Player, PlayerDead,

    Pipe, PipeSwish,

    PlayerJump, PlayerRespawn,



    Rat, RatDead,

    Bat, BatDead,

    Enemy, EnemyDead,
    EnemyGun, EnemyBullet
}*/


[System.Serializable]
public class GameObjectPrefab
{
    public enum ObjectFrom { Prefab, Name, Label }
    public ObjectFrom objectFrom = ObjectFrom.Prefab;
    
    public GameObject prefab;
    public string name;
    //public ObjectLabel objLabel;

    public GameObject Instantiate()
    {
        switch (objectFrom)
        {
            case ObjectFrom.Prefab:
                return ObjectManager.Instantiate(prefab);
            case ObjectFrom.Name:
                return ObjectManager.Instantiate(name);
            //case ObjectFrom.Label:
            //    return ObjectManager.Instantiate(objLabel);
        }
        return null;
    }
}


public class CorpseManager
{
    public static Transform parent;

    public static GameObject prefab;
    public static List<SpriteRenderer> pooledObjects;

    const int prewarm = 100;


    public static void Initialize(GameObject corpsePrefab)
    {
        prefab = corpsePrefab;
        pooledObjects = new List<SpriteRenderer>();
        CreateParentTransform();

        for (int i = 0; i < prewarm; i++)
        {
            SpriteRenderer newObject = NewObject();
            newObject.GetComponent<PooledObject>().isPreWarm = true;
            AddObject(newObject.gameObject);
        }
    }

    public static void CreateParentTransform()
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "Corpse Pool";
        parentObject.transform.parent = ObjectPool.Root;
        parentObject.transform.Zero();

        parent = parentObject.transform;
    }

    public static void CreateCorpse(GameObject source)
    {
        SpriteRenderer sourceSprite = source.GetComponent<SpriteRenderer>();
        if(sourceSprite != null)
        {
            CreateCorpse(sourceSprite);
        }
        else
        {
            Debug.LogError("Error: Could not find sprite to corpse in " + source);
        }
    }

    public static void CreateCorpse(SpriteRenderer source)
    {
        SpriteRenderer returnObject;

        if (pooledObjects.Count > 0)
        {
            returnObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);
            returnObject.gameObject.SetActive(true);
        }
        else
        {
            returnObject = NewObject();
        }

        returnObject.sprite = source.sprite;
        returnObject.color = source.color;
        returnObject.flipX = source.flipX;
        returnObject.flipY = source.flipY;

        returnObject.name = source.name + " (Corpse)";

        returnObject.transform.parent = source.transform.parent;
        returnObject.transform.localPosition = source.transform.localPosition;
        returnObject.transform.localRotation = source.transform.localRotation;
        returnObject.transform.localScale = source.transform.localScale;

        ObjectManager.Destroy(source.gameObject);
    }

    public static void AddObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = parent;
        pooledObjects.Add(gameObject.GetComponent<SpriteRenderer>());
    }


    
    public static SpriteRenderer NewObject()
    {
        GameObject newObject = GameObject.Instantiate(prefab) as GameObject;
        
        PooledObject newPooledObj = newObject.GetComponent<PooledObject>();
        if (newPooledObj == null) newPooledObj = newObject.AddComponent<PooledObject>();

        newPooledObj.isCorpse = true;

        return newObject.GetComponent<SpriteRenderer>();
    }

}


[System.Serializable]
public class ObjectPool
{
    [HideInInspector]
    public string name;

    private static Transform root;
    public static Transform Root
    {
        get
        {
            if(root == null)
            {
                GameObject rootObj = new GameObject();
                rootObj.name = "Object Pools";
                root = rootObj.transform;
            }
            return root;
        }
    }

    
    public GameObject prefab;

    public int prewarm = 0;

    public int numActive = 0;
    public int maxActive = 0;

    [System.NonSerialized]
    public Transform parent;

    [System.NonSerialized]
    public List<GameObject> pooledObjects;
    

    public void OnValidate()
    {
        if (prefab != null) name = prefab.name;
    }
    
    public ObjectPool()
    {
        //Initialize();
    }

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;

        Initialize();
    }

    public virtual void Initialize()
    {
        pooledObjects = new List<GameObject>();
        CreateParentTransform();

        for (int i = 0; i < prewarm; i++)
        {
            GameObject newObject = NewObject();
            newObject.GetComponent<PooledObject>().isPreWarm = true;
            AddObject(newObject);
        }
    }

    public void CreateParentTransform()
    {
        GameObject parentObject = new GameObject();
        parentObject.name = prefab.name + " Pool";
        parentObject.transform.parent = Root;
        parentObject.transform.Zero();

        parent = parentObject.transform;
    }

    public GameObject PullObject()
    {
        return PullObject(Vector3.zero);
    }
    public GameObject PullObject(Vector3 position)
    {
        GameObject returnObject;
        if (pooledObjects.Count > 0)
        {
            returnObject = pooledObjects[0];
            pooledObjects.RemoveAt(0);

            returnObject.transform.parent = null;
            returnObject.transform.position = position;

            returnObject.SetActive(true);

            if (returnObject.GetComponent<PooledObject>().isPreWarm)
            {
                returnObject.GetComponent<PooledObject>().isPreWarm = false;
            }
            else
            {
                returnObject.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            returnObject = NewObject(position);

            if (MyTime.isGameStateStable)
            {
                //Debug.Log("Uh oh! Instantiation happened at runtime for " + returnObject);
            }
        }

        numActive++;
        maxActive = Mathf.Max(maxActive, numActive);

        return returnObject;
    }

    public void AddObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.parent = parent;
        pooledObjects.Add(gameObject);

        if(pooledObjects.Count > prewarm)
        {
            prewarm = pooledObjects.Count;

            if (MyTime.isGameStateStable)
            {
                Debug.Log("Capacity exceeds prewarm. Consider increasing prewarm to " + pooledObjects.Count + " for " + prefab.name + " pool");
            }
        }

        numActive = Mathf.Max(0, numActive - 1);
    }



    public GameObject NewObject()
    {
        return NewObject(Vector3.zero);
    }

    public GameObject NewObject(Vector3 position)
    {
        GameObject newObject = GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;
        //newObject.SetActive(false);

        newObject.name = prefab.name;

        PooledObject newPooledObj = newObject.GetComponent<PooledObject>();
        if (newPooledObj == null) newPooledObj = newObject.AddComponent<PooledObject>();

        newPooledObj.parentPool = this;
        newPooledObj.dirty = false;

        return newObject;
    }
    
}




public class ObjectManager : Singleton<ObjectManager> {
    
    

    public static GameObject Instantiate(string name)
    {
        return Instantiate(name, Vector3.zero);
    }
    public static GameObject Instantiate(string name, Vector3 position)
    {
        if (nameDic.ContainsKey(name))
        {
            return Instantiate(nameDic[name], position);
        }
        else
        {
            Debug.LogError("GameObject with name \"" + name + "\" not found");
            return null;
        }
    }
    
    public static GameObject Instantiate(GameObject prefab)
    {
        return Instantiate(prefab, Vector3.zero);
    }
    public static GameObject Instantiate(GameObject prefab, Vector3 position)
    {
        GameObject newObject;
        if (prefab.GetComponent<PooledObject>() != null)
        {
            if (!objectPools.ContainsKey(prefab))
            {
                objectPools.Add(prefab, new ObjectPool(prefab));
            }

            newObject = objectPools[prefab].PullObject(position);
        }
        else
        {
            newObject = GameObject.Instantiate(prefab, position, Quaternion.identity) as GameObject;
            newObject.name = prefab.name;

            if (MyTime.isGameStateStable)
            {
                //Debug.Log("Uh oh! Instantiation happened at runtime for " + newObject);
            }

        }

        newObject.transform.parent = Instance.commonParent;

        return newObject;
    }
    

    public static void Destroy(GameObject gameObject)
    {
        PooledObject pooledObject = gameObject.GetComponent<PooledObject>();
        
        if(pooledObject != null)
        {
            if (pooledObject.isCorpse)
            {
                CorpseManager.AddObject(gameObject);
                return;
            }

            if (!pooledObject.dirty)
            {
                pooledObject.parentPool.AddObject(gameObject);
                return;
            }
        }
        
        UnityEngine.Object.Destroy(gameObject);
    }


    public static Dictionary<GameObject, ObjectPool> objectPools;



    public static Dictionary<string, GameObject> nameDic;

    public static void CompileDictionaries()
    {
        nameDic = new Dictionary<string, GameObject>();

        objectPools = new Dictionary<GameObject, ObjectPool>();

        for (int i = 0; i < Instance.prefabs.Length; i++)
        {
            nameDic.Add(Instance.prefabs[i].name, Instance.prefabs[i]);
        }

        for (int i = 0; i < Instance.presetPools.Length; i++)
        {
            if (!objectPools.ContainsKey(Instance.presetPools[i].prefab)){
                Instance.presetPools[i].Initialize();
                objectPools.Add(Instance.presetPools[i].prefab, Instance.presetPools[i]);
            }
        }

        CorpseManager.Initialize(Instance.corpse);
    }


    
    public Transform commonParent;

    public GameObject[] prefabs;
    public ObjectPool[] presetPools;

    public GameObject corpse;
    
    void Awake()
    {
        MyTime.MarkAsUnstable();

        CompileDictionaries();
        
        MyTime.MarkAsStable();
    }

    void OnValidate()
    {
        for (int i = 0; i < presetPools.Length; i++) presetPools[i].OnValidate();
    }

    public void CleanUpObjects()
    {
        Transform child;
        for(int i=0; i<commonParent.childCount; i++)
        {
            child = commonParent.GetChild(i);
            ObjectManager.Destroy(child.gameObject);

            if (child.parent != commonParent) i--;
        }

    }


}
