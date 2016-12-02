using UnityEngine;
using System.Collections;

public class Follow2D : Follow {

    private Vector2 velocity;

    private float lockedZPosition;

    public override void Start()
    {
        lockedZPosition = transform.position.z;

        base.Start();
    }


    public override void UpdatePositionLerp()
    {
        position = Vector2.Lerp(position, tPosition, lerpSpeed * deltaTime);
    }

    public override void UpdatePositionLerpCorrected()
    {
        position = Vector2.Lerp(position, tPosition, Com.CorrectLerpFactor(lerpFactor, deltaTime));
    }
    
    public override void UpdatePositionSmoothDamp()
    {
        position = Vector2.SmoothDamp(position, tPosition, ref velocity, smoothTime, Mathf.Infinity, deltaTime);
    }

    public override void UpdatePositionMoveTowards()
    {
        position = Vector2.MoveTowards(position, tPosition, moveSpeed * deltaTime);
    }


    public override void ValidatePosition()
    {
        position.z = lockedZPosition;
    }

}
