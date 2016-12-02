using UnityEngine;
using System.Collections;

public class FreezeFrames : Singleton<FreezeFrames>
{
    const float timePerFrame = 0.0167f;

    const float stunnedTimeScale = 0.000f;

    const int minStunFrames = 2;
    const float minStunTime = .0167f;
    const int maxStunFrames = 30;//10;
    const float maxStunTime = .2f;//0.1667f;

    //const int fpsThreshold = 20;
    

    static float stuns = 0;
    static float stunTime = 0f;

    public static void Stun(float frames = 1f)
    {
        //if (GameSettings.screenEffectsEnabled && 1f / MyTime.avgUnscaledDeltaTime > fpsThreshold)
        if (GameSettings.screenEffectsEnabled)
        {
            if (MyTime.isFramerateStable)
            {
                stuns += frames;
                stunTime += (frames * timePerFrame);

                if (!Instance.stunned)
                {
                    Instance.StartCoroutine(Instance.StartStun());
                }
            }
            else
            {
                Debug.Log("Framerate too unstable for stun frames, skipping");
            }

        }
    }

    public bool stunned = false;
    float timeScale = 1f;

    float stunnedTime;
    float stunnedFrames;

    void Awake () {
		//FreezeFrames.instance = this;
	}
    

    public IEnumerator StartStun()
    {
        stunned = true;

        stunTime = Mathf.Max(minStunTime, stunTime);

        stunnedFrames = 0;
        stunnedTime = .5f * MyTime.UnscaledDeltaTime;


        yield return new WaitForEndOfFrame();

        timeScale = Time.timeScale;
        Time.timeScale = stunnedTimeScale;

        yield return StartCoroutine(StunLoop());
        
        EndStun();
    }

    public IEnumerator StunLoop()
    {
        //while ((stunnedTime < stunTime || stunnedFrames < minStunFrames) && stunnedTime < maxStunTime && stunnedFrames < maxStunFrames)
        while ((stunnedTime < stunTime || stunnedFrames < minStunFrames) && stunnedTime < maxStunTime)
        {
            yield return new WaitForEndOfFrame();
            stunnedFrames++;
            stunnedTime += Time.unscaledDeltaTime;
        }
    }
    
    public void EndStun()
    {
        //stunnedTime -= .5f * MyTime.avgUnscaledDeltaTime;
        //Debug.Log("Prompt: " + stuns + " frames, " + stunTime.ToString("0.0000") + " seconds. Result: " + stunnedFrames + " frames, " + stunnedTime.ToString("0.0000") + " seconds (" + (stunnedTime * 60f).ToString("0.0") + " frames @60fps)");
        
        stuns = 0f;
        stunTime = 0f;

        if(Game.gameState == GameState.Play)
            Time.timeScale = timeScale;

        stunned = false;
    }
}
