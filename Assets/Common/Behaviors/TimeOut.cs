using UnityEngine;
using System.Collections;

[AddComponentMenu("Common/Timeout")]
public class TimeOut : MonoBehaviour {

    public enum OnTimeout { Destroy, Disable, Corpse }
    public OnTimeout onTimeout = OnTimeout.Destroy;

    //public enum TimeoutBehavior { Time, Frames, Both, Either }
    //public TimeoutBehavior behavior = TimeoutBehavior.Time;

    public RandomizedFloat time = 1.0f;
    //public float variance = 0f;
    //public float time = 1.0f;
    //public int frames = 1;
    private float startTime = 0f;
    private float timeoutTime = -1f;


    public TimeoutEffect.TimeoutEffectLabel timeoutEffectLabel = TimeoutEffect.TimeoutEffectLabel.None;
    private TimeoutEffect timeoutEffect;
    
    void Awake()
    {
        timeoutEffect = TimeoutEffect.New(timeoutEffectLabel);
    }

    void Start()
    {
        timeoutEffect.Initialize(this);

        startTime = Time.time;
        timeoutTime = startTime + time;
    }
    
	void Update()
    {
        if (Time.timeScale > 0f)
        {
            timeoutEffect.Update(Mathf.InverseLerp(startTime, timeoutTime, Time.time));

            if (Time.time > timeoutTime) Timeout();
        }
    }


    void Timeout()
    {
        switch (onTimeout)
        {
            case OnTimeout.Destroy:
                ObjectManager.Destroy(gameObject);
                break;
            case OnTimeout.Disable:
                gameObject.SetActive(false);
                break;
            case OnTimeout.Corpse:
                CorpseManager.CreateCorpse(gameObject);
                break;
        }
    }


}



public class TimeoutEffect
{
    public enum TimeoutEffectLabel { None, Blink, Fade }

    public static TimeoutEffect New(TimeoutEffectLabel behavior)
    {
        switch (behavior)
        {
            case TimeoutEffectLabel.None:
                return new TimeoutEffect();
            case TimeoutEffectLabel.Blink:
                return new TimeoutEffectBlink();
            case TimeoutEffectLabel.Fade:
                return new TimeoutEffectFade();
        }
        return null;
    }


    
    public TimeOut timeout;

    public virtual void Initialize(TimeOut timeout)
    {
        this.timeout = timeout;
    }

    public virtual void Update(float t)
    {

    }
}

public class TimeoutEffectBlink : TimeoutEffect
{
    public Renderer renderer;

    public override void Initialize(TimeOut timeout)
    {
        base.Initialize(timeout);
        renderer = timeout.GetComponent<Renderer>();
    }

    public override void Update(float t)
    {
        float blinkInterval = 1f - t;
        renderer.enabled = (t % (2f * blinkInterval) < blinkInterval);
    }
}

public class TimeoutEffectFade : TimeoutEffect
{
    public SpriteRenderer renderer;
    Color color;
    float iAlpha;

    public override void Initialize(TimeOut timeout)
    {
        base.Initialize(timeout);
        renderer = timeout.GetComponent<SpriteRenderer>();
        color = renderer.color;
        iAlpha = renderer.color.a;
    }

    public override void Update(float t)
    {
        if (t > .5f)
        {
            color.a = iAlpha * Mathf.Lerp(2f, 0f, t);
            renderer.color = color;
        }
    }
}