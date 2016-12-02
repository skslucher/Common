using UnityEngine;
using System.Collections;

public class RenderTextureMouseEvents : Singleton<RenderTextureMouseEvents> {


    public class MouseEventObject
    {
        Collider overCollider;
        Camera overCamera;

        Collider clickCollider;
        Camera clickCamera;

        public MouseEventObject()
        {
            overCollider = null;
            overCamera = null;
            clickCollider = null;
            clickCamera = null;
        }

        public void Update(Collider collider, Camera camera)
        {
            // Over events // Enter - Over - Exit

            if (overCollider != null)
            {
                if (overCollider != collider)
                {
                    overCollider.gameObject.SendMessage("OnMouseExit", overCamera, SendMessageOptions.DontRequireReceiver);

                    overCollider = null;
                    overCamera = null;
                }
            }

            if (collider != null)
            {
                if (overCollider == null)
                {
                    overCollider = collider;
                    overCamera = camera;

                    overCollider.gameObject.SendMessage("OnMouseEnter", overCamera, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    overCollider.gameObject.SendMessage("OnMouseStay", overCamera, SendMessageOptions.DontRequireReceiver);
                }
            }

            // Click events // Down - Drag - Up

            if (clickCollider != null)
            {
                if (Input.GetMouseButton(0))
                {
                    clickCollider.gameObject.SendMessage("OnMouseDrag", clickCamera, SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    clickCollider.gameObject.SendMessage("OnMouseUp", clickCamera, SendMessageOptions.DontRequireReceiver);
                    if (collider == clickCollider)
                    {
                        clickCollider.gameObject.SendMessage("OnMouseUpAsButton", clickCamera, SendMessageOptions.DontRequireReceiver);
                    }
                    clickCollider = null;
                    clickCamera = null;
                }
            }
            else
            {
                if (collider != null && Input.GetMouseButtonDown(0))
                {
                    clickCollider = collider;
                    clickCamera = camera;

                    clickCollider.gameObject.SendMessage("OnMouseDown", clickCamera, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }



    public static Vector3 MousePosition()
    {
        Vector2 mousePosition = Instance.thisCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition = Instance.renderTextureScale.InverseTransformPoint(mousePosition);
        mousePosition += Vector2.one * .5f;

        mousePosition = Vector3.Scale(mousePosition, new Vector2(Instance.renderTexture.width, Instance.renderTexture.height));

        return Instance.guiCamera.ScreenToWorldPoint(mousePosition);
    }
    



    public RenderTexture renderTexture;

    public Transform renderTextureScale;

    public Camera thisCamera;
    public Camera guiCamera;
    
    private MouseEventObject mouseEventObject;

    void Awake()
    {
        mouseEventObject = new MouseEventObject();
    }

    public void Update()
    {
        HandleMouseEvents();
    }

    public void HandleMouseEvents()
    {
        if (guiCamera != null && guiCamera.isActiveAndEnabled)
        {
            Vector2 mousePosition = thisCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePosition = renderTextureScale.InverseTransformPoint(mousePosition);
            mousePosition += Vector2.one * .5f;

            if (new Rect(0f, 0f, 1f, 1f).Contains(mousePosition))
            {
                mousePosition = Vector3.Scale(mousePosition, new Vector2(renderTexture.width, renderTexture.height));
                
                Ray raycast = guiCamera.ScreenPointToRay(mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(raycast, out hit, guiCamera.farClipPlane, guiCamera.cullingMask))
                {
                    mouseEventObject.Update(hit.collider, guiCamera);
                    return;
                }

                
            }
        }

        mouseEventObject.Update(null, null);
    }
   





}
