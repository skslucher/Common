using UnityEngine;
using System.Collections;

//theadrainblog.wordpress.com

[ExecuteInEditMode]
public class PixelPerfectScaleUI : MonoBehaviour
{
    public enum FitModes { BEST_FIT, MAINTAIN_ASPECT, SCALE_TO_FIT }
    public FitModes fitMode = FitModes.BEST_FIT;

    public float gameHorizontalPixels;
    public float gameVerticalPixels;
    float minimumMultiplier = 1;
    public float pixelPerUnit = 16f;

    private float screenPixelsY = 0;

    void Update()
    {
        if (screenPixelsY != (float)Screen.height)
        {
            switch (fitMode)
            {
                case FitModes.BEST_FIT:
                    BestFit();
                    break;
                case FitModes.MAINTAIN_ASPECT:
                    MaintainAspectFit();
                    break;
                case FitModes.SCALE_TO_FIT:
                    ScaleToFit();
                    break;
            }
        }
    }

    private void BestFit()
    {
        float targetHeight = gameVerticalPixels;
        float multiplier = minimumMultiplier;

        multiplier = screenPixelsY / targetHeight;
        multiplier -= multiplier % 2;
        if (multiplier < 2)
        {
            multiplier = minimumMultiplier;
        }

        float aspect = gameHorizontalPixels / gameVerticalPixels;
        float height = gameVerticalPixels * multiplier;
        float width = height * aspect;

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void MaintainAspectFit()
    {
        float aspect = gameHorizontalPixels / gameVerticalPixels;

        float targetWidth;
        float targetHeight;

        if (Screen.width < Screen.height * aspect)
        {
            // fit to width
            targetWidth = Screen.width;
            targetHeight = Screen.width / aspect;
        }
        else
        {
            // fit to height
            targetHeight = Screen.height;
            targetWidth = Screen.height * aspect;
        }

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight / pixelPerUnit);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth / pixelPerUnit);
    }

    private void ScaleToFit()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / pixelPerUnit);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / pixelPerUnit);
    }
}
