using UnityEngine;
using System.Collections;

public class Follow3D : Follow {

    private Vector3 velocity;


    public override void UpdatePositionLerp()
    {
        position = Vector3.Lerp(position, tPosition, lerpSpeed * deltaTime);
    }

    public override void UpdatePositionLerpCorrected()
    {
        position = Vector3.Lerp(position, tPosition, Com.CorrectLerpFactor(lerpFactor, deltaTime));
    }

    public override void UpdatePositionSmoothDamp()
    {
        position = Vector3.SmoothDamp(position, tPosition, ref velocity, smoothTime, Mathf.Infinity, deltaTime);
    }

    public override void UpdatePositionMoveTowards()
    {
        position = Vector3.MoveTowards(position, tPosition, moveSpeed * deltaTime);
    }

}
