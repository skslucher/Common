using UnityEngine;
using System.Collections;

public class RTGraphicRaycaster : UnityEngine.UI.GraphicRaycaster
{
    public Camera raycastCamera;

    public override Camera eventCamera
    {
        get
        {
            return raycastCamera;
        }
    }
}
