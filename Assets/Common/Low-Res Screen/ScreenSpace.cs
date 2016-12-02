using UnityEngine;
using System.Collections;

//Based on PixelPerfectScaleUI.cs by theadrainblog.wordpress.com

[ExecuteInEditMode]
public class ScreenSpace : MonoBehaviour {

    const string screenSpaceTag = "Finish";


    public Camera screenCamera;
    public Transform screenScale;
    public RenderTexture screenRT;
    
    public enum FitModes { BEST_FIT, MAINTAIN_ASPECT, SCALE_TO_FIT }
    public FitModes fitMode = FitModes.MAINTAIN_ASPECT;

    private Vector2 screenSize;
    
    //void OnValidate()
    //{
    //    Refresh();
    //}


    //void Start()
    //{
    //    Refresh();
    //}





    void Update()
    {
        if(screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            Refresh();
        }


        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            screenCamera.enabled = !screenCamera.enabled;
        }
    }



    void Refresh()
    {
        screenSize = new Vector2(Screen.width, Screen.height);

        RefreshScale();
    }

    void RefreshScale()
    {
        Vector2 targetScale = Vector2.one;
        switch (fitMode)
        {
            case FitModes.BEST_FIT:
                targetScale = BestFit();
                break;
            case FitModes.MAINTAIN_ASPECT:
                targetScale = MaintainAspectFit();
                break;
            case FitModes.SCALE_TO_FIT:
                targetScale = ScaleToFit();
                break;
        }
        targetScale *= toWorld / toScreen;
        screenScale.localScale = (Vector3)targetScale + Vector3.forward;
    }



    private Vector2 BestFit()
    {
        Vector2 targetSize = MaintainAspectFit();// * toScreen / toWorld;

        float factor = targetSize.y / screenRT.height;

        factor = Mathf.Floor(factor);
        
        if(factor > 0f)
        {
            targetSize = factor * new Vector2(screenRT.width, screenRT.height);
        }

        return targetSize;
    }

    private Vector2 MaintainAspectFit()
    {
        float rtAspect = (float)screenRT.width / screenRT.height;
        
        float screenHeight = Mathf.Min(screenSize.x / rtAspect, screenSize.y); //Pixels

        return new Vector2(screenHeight * rtAspect, screenHeight); //Pixels
    }

    private Vector2 ScaleToFit()
    {
        //Vector2 targetSize = new Vector2(Screen.width, Screen.height); //Pixels
        
        //return targetSize;
        return screenSize;
    }
    


    public float toScreen
    {
        get
        {
            return Screen.height;
        }
    }

    public float toWorld
    {
        get
        {
            return 2f * screenCamera.orthographicSize;
        }
    }




}
