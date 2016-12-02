using UnityEngine;
using System.Collections;
using SKSCommon;

public enum FollowTarget { Position, Transform, Tag, Player }
public enum FollowBehavior { None, Lerp, LerpCorrected, SmoothDamp, MoveTowards }

public class Follow : MonoBehaviour
{
    [HideInInspector]
    public Vector3 position;
    

    [Header("Target")]
    public FollowTarget target = FollowTarget.Transform;

    [ConditionalHideInt("target", (int)FollowTarget.Position, ConditionalHideBehavior.Hide)]
    public Vector3 tPosition = Vector3.zero;
    [ConditionalHideInt("target", (int)FollowTarget.Transform, ConditionalHideBehavior.Hide)]
    public Transform tTransform;
    [ConditionalHideInt("target", (int)FollowTarget.Tag, ConditionalHideBehavior.Hide)]
    public string tTag = "Player";
    
    public bool destroyWhenTargetless = false;



    [Header("Behavior")]
    public FollowBehavior behavior = FollowBehavior.Lerp;

    [ConditionalHideInt("behavior", (int)FollowBehavior.Lerp, ConditionalHideBehavior.Hide)]
    public float lerpSpeed = 10f; //For Lerp
    [ConditionalHideInt("behavior", (int)FollowBehavior.LerpCorrected, ConditionalHideBehavior.Hide)]
    public float lerpFactor = .98f; //For LerpCorrected
    [ConditionalHideInt("behavior", (int)FollowBehavior.SmoothDamp, ConditionalHideBehavior.Hide)]
    public float smoothTime = .1f; //For SmoothDamp
    [ConditionalHideInt("behavior", (int)FollowBehavior.MoveTowards, ConditionalHideBehavior.Hide)]
    public float moveSpeed = 5f; //For MoveTowards


    public enum UpdateInterval { Update, LateUpdate, FixedUpdate }
    [Header("Misc")]
    public UpdateInterval updateInterval = UpdateInterval.LateUpdate;
    
    public bool startOnTarget = true;
    
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
    


    public virtual void Start()
    {
        position = transform.position;
        
        if (tTransform != null && target != FollowTarget.Transform) tTransform = null;

        UpdateTarget();

        if (tPosition == Vector3.zero && target != FollowTarget.Position)
        {
            tPosition = transform.position;
        }

        if (startOnTarget)
        {
            transform.position = tPosition;
        }
    }

    void Update()
    {
        if (updateInterval == UpdateInterval.Update)
        {
            UpdateTarget();
            UpdatePosition();
        }
    }
    
    void LateUpdate()
    {
        if (updateInterval == UpdateInterval.LateUpdate)
        {
            UpdateTarget();
            UpdatePosition();
        }
    }

    void FixedUpdate()
    {
        if (updateInterval == UpdateInterval.FixedUpdate)
        {
            UpdateTarget();
            UpdatePosition();
        }
    }


    public void UpdateTarget()
    {
        if (target != FollowTarget.Position)
        {
            //if (tTransform == null)
            {
                UpdateTargetTransform();
            }

            if (tTransform == null && destroyWhenTargetless)
            {
                ObjectManager.Destroy(gameObject);
            }

            UpdateTargetPosition();
        }
    }

    public void UpdateTargetTransform()
    {
        switch (target)
        {
            case FollowTarget.Tag:
                if (tTransform == null || tTransform.gameObject.tag != tTag)
                {
                    tTransform = null;
                    GameObject tObj = GameObject.FindWithTag(tTag);
                    if (tObj != null)
                    {
                        tTransform = GameObject.FindWithTag(tTag).transform;
                    }
                }
                break;
            case FollowTarget.Player:
                //if (Global.player != null)
                {
                    tTransform = Game.Player;
                }
                break;
        }
    }

    public void UpdateTargetPosition()
    {
        if (tTransform != null)
        {
            tPosition = tTransform.position;
        }
    }

    

    public void UpdatePosition()
    {
        UpdatePositionFromBehavior();

        ValidatePosition();

        ApplyPosition();
    }


    public void UpdatePositionFromBehavior()
    {
        switch (behavior)
        {
            case FollowBehavior.None:
                position = tPosition;
                break;
            case FollowBehavior.Lerp:
                UpdatePositionLerp();
                break;
            case FollowBehavior.LerpCorrected:
                UpdatePositionLerpCorrected();
                break;
            case FollowBehavior.SmoothDamp:
                UpdatePositionSmoothDamp();
                break;
            case FollowBehavior.MoveTowards:
                UpdatePositionMoveTowards();
                break;
        }
    }
    
    public virtual void UpdatePositionLerp() { }
    public virtual void UpdatePositionLerpCorrected() { }
    public virtual void UpdatePositionSmoothDamp() { }
    public virtual void UpdatePositionMoveTowards() { }
    
    public virtual void ValidatePosition() { }

    public virtual void ApplyPosition()
    {
        transform.position = position;
    }


}
