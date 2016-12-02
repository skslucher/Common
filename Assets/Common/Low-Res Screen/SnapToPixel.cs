using UnityEngine;
using System.Collections;

[AddComponentMenu("Common/Snap To Pixel")]
public class SnapToPixel : MonoBehaviour {

    const float pixelsPerUnit = 16f;
    const float unitsPerPixel = 0.0625f;
    


    public float speedThreshold = .05f;
    public new Rigidbody2D rigidbody;

    public bool whenSleeping = true;
    private bool isSleeping
    {
        get
        {
            return whenSleeping && rigidbody != null && rigidbody.IsSleeping();
        }
    }

    public bool whenUnderThreshold = true;
    private bool isUnderThreshold
    {
        get
        {
            if (!whenUnderThreshold) return false;

            if(rigidbody == null)
            {
                return rigidbody.velocity.magnitude < speedThreshold;
            }
            else
            {
                return Vector3.Distance(oldPosition, transform.position) < speedThreshold * Time.fixedDeltaTime;
            }

        }
    }

    
    public bool snapPermanently = false;

    public bool corpseOnSnap = false;

    Transform target
    {
        get
        {
            if (rigidbody != null)
            {
                return rigidbody.transform;
            }
            else
            {
                return transform;
            }
        }
    }
    private bool isTargetingSelf
    {
        get
        {
            return target == transform;
        }
    }


    Vector3 oldPosition;

    Vector3 snapPosition;
    Quaternion snapRotation;

    


    void OnEnable()
    {
        if(rigidbody != null)
            rigidbody.isKinematic = false;

        //snapPosition = Com.Round(target.position, unitsPerPixel);

        //Vector3 snapRotationEuler = target.rotation.eulerAngles;
        //snapRotationEuler.z = Com.Round(snapRotationEuler.z, 90f);
        //snapRotation = Quaternion.Euler(snapRotationEuler);
    }


    void FixedUpdate()
    {
        if(snapPosition != transform.position)
        {
            if (Vector3.Distance(snapPosition, target.position) < unitsPerPixel * .5f)
            {
                transform.position = snapPosition;
                transform.rotation = snapRotation;
            }
            else
            {
                if (isSleeping || isUnderThreshold)
                {
                    Snap();
                }
                else
                {
                    if (!isTargetingSelf)
                    {
                        transform.position = target.position;
                        transform.rotation = target.rotation;
                    }
                }
            }

        }

        oldPosition = transform.position;
    }


    void Snap()
    {
        if (snapPermanently && rigidbody != null)
        {
            rigidbody.isKinematic = true;
            //DestroyImmediate(rigidbody);
        }


        if (snapPosition != target.position)
        {
            snapPosition = Com.Round(target.position, unitsPerPixel);
        }

        if (snapRotation != target.rotation)
        {
            Vector3 snapRotationEuler = target.rotation.eulerAngles;
            snapRotationEuler.z = Com.Round(snapRotationEuler.z, 90f);
            snapRotation = Quaternion.Euler(snapRotationEuler);
        }


        transform.position = snapPosition;
        transform.rotation = snapRotation;

        if (snapPermanently)
        {
            if (corpseOnSnap)
            {
                CorpseManager.CreateCorpse(gameObject);
            }
            //Destroy(this);
        }
    }

}
