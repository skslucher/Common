using UnityEngine;
using System.Collections;

public class Screenshake : MonoBehaviour
{
    public static Screenshake Instance;

    
    static float intensity = 0f;
    const float intensityMax = 5f;
    const float intensityFalloff = 5f;

    static float intensityScale = .25f; //For settings

    const float shakeSpeed = 10f;

    public static void Shake(float shakeIntensity)
    {
        if (GameSettings.screenEffectsEnabled)
        {
            intensity = Mathf.Min(intensity + shakeIntensity, intensityMax);
        }
	}


    static Vector2 kick = Vector2.zero;
    const float kickMax = 1f;
    const float kickFalloff = 5f;

    static float kickScale = 1f; //For settings

    public static void Kick(Vector2 kickDirection)
    {
        if (GameSettings.screenEffectsEnabled)
        {
            kick = Vector2.ClampMagnitude(kick + kickDirection, kickMax);
        }
	}


	
	private float t;
    private Vector3 zero;
    
    void Awake()
    {
        Instance = this;
        zero = transform.localPosition;
        t = 0f;
    }

	void Update()
    {
        if (Time.timeScale > 0f)
        {
            Vector3 position = zero;

            if(kick != Vector2.zero)
            {
                position += (Vector3)(kick * kickScale);
                kick = Vector2.MoveTowards(kick, Vector2.zero, kickFalloff * Time.deltaTime);
            }

            if(intensity != 0f)
            {
                t += Time.deltaTime * shakeSpeed;

                position += (Vector3)(intensity * intensityScale * PerlinUtil.Variance2D(t));
                intensity = Mathf.MoveTowards(intensity, 0f, intensityFalloff * Time.deltaTime);
            }

            transform.localPosition = position;

        }
	}
}
