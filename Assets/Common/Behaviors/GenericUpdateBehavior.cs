using UnityEngine;
using System.Collections;

public class GenericUpdateBehavior : MonoBehaviour {

    public enum UpdateInterval { Update, LateUpdate, FixedUpdate }
    public UpdateInterval updateInterval = UpdateInterval.Update;
    
    public float deltaTime
    {
        get
        {
            switch (updateInterval)
            {
                case UpdateInterval.Update:
                    return Time.deltaTime;
                case UpdateInterval.LateUpdate:
                    return Time.deltaTime;
                case UpdateInterval.FixedUpdate:
                    return Time.fixedDeltaTime;
                default:
                    return 0f;
            }
        }
    }


    void Update()
    {
        if (updateInterval == UpdateInterval.Update)
        {
            UpdateBehavior();
        }
    }

    void LateUpdate()
    {
        if (updateInterval == UpdateInterval.LateUpdate)
        {
            UpdateBehavior();
        }
    }

    void FixedUpdate()
    {
        if (updateInterval == UpdateInterval.FixedUpdate)
        {
            UpdateBehavior();
        }
    }


    public virtual void UpdateBehavior()
    {

    }

}
