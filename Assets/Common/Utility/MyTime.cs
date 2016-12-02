using UnityEngine;
using System.Collections;

public class MyTime : Singleton<MyTime> {

    const int targetFramerate = 60;
    const int stableThreshold = 40;
    const int frameBuffer = 5;
    const int bufferBuffer = 5;

    const float maxTimeout = 1f;

    public static float DeltaTime
    {
        get
        {
            return Instance.deltaTime;
        }
    }
    
    public static float AvgDeltaTime
    {
        get
        {
            float total = 0f;
            foreach (float deltaTime in Instance.deltaTimes) total += deltaTime;
            return total / frameBuffer;
        }
    }

    public static float UnscaledDeltaTime
    {
        get
        {
            return Instance.unscaledDeltaTime;
        }
    }

    public static float AvgUnscaledDeltaTime
    {
        get
        {
            float total = 0f;
            foreach (float deltaTime in Instance.unscaledDeltaTimes) total += deltaTime;
            return total / frameBuffer;
        }
    }
    

    public static float FPS
    {
        get
        {
            return 1f / AvgUnscaledDeltaTime;
        }
    }
    
    public static bool isFramerateStable
    {
        get
        {
            //Debug.Log("Max delta: " + Instance.maxDeltaTime + ", Average delta: " + AvgUnscaledDeltaTime);
            //return Instance.maxDeltaTime < stableThreshold / targetFramerate;
            //return Instance.maxDeltaTime < AvgUnscaledDeltaTime * stableThreshold;
            return Instance.maxDeltaTime < 1f / stableThreshold;
        }
    }
    


    public static bool isGameStateStable
    {
        get
        {
            return stableFlags == 0;
        }
    }

    private static int stableFlags = 0;
    public static void MarkAsUnstable()
    {
        stableFlags++;
    }
    public static void MarkAsStable()
    {
        stableFlags = Mathf.Max(0, stableFlags - 1);
    }


    static public void Pause()
    {
        Time.timeScale = 0f;
    }

    static public void Unpause()
    {
        Time.timeScale = 1f;
    }



    float accumDeltaTime;
    float deltaTime;
    float[] deltaTimes;

    float accumUnscaledDeltaTime;
    float unscaledDeltaTime;
    float[] unscaledDeltaTimes;
    int frameIndex = 0;
    int bufferIndex = 0;

    public float maxDeltaTime;
    float maxTime;
    public float minDeltaTime;
    float minTime;

    void Awake()
    {
        //MarkAsUnstable();
        //Application.targetFrameRate = targetFramerate;
        //QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = -1;

        deltaTimes = new float[frameBuffer];
        for (int i = 0; i < frameBuffer; i++) deltaTimes[i] = 1f / targetFramerate;
        unscaledDeltaTimes = new float[frameBuffer];
        for (int i = 0; i < frameBuffer; i++) unscaledDeltaTimes[i] = 1f / targetFramerate;

        maxDeltaTime = .01f;
        maxTime = 0f;

        accumDeltaTime = 0f;
        accumUnscaledDeltaTime = 0f;

        frameIndex = 0;
        bufferIndex = 0;
    }

    void Start()
    {
        Application.targetFrameRate = -1;
    }


	void Update ()
    {
        if (isGameStateStable)
        {

            accumDeltaTime += Time.deltaTime;
            accumUnscaledDeltaTime += Time.unscaledDeltaTime;
            frameIndex++;


            if(frameIndex == frameBuffer)
            {
                deltaTime = accumDeltaTime / frameBuffer;
                deltaTimes[bufferIndex] = deltaTime;
                accumDeltaTime = 0f;

                unscaledDeltaTime = accumUnscaledDeltaTime / frameBuffer;
                unscaledDeltaTimes[bufferIndex] = unscaledDeltaTime;
                accumUnscaledDeltaTime = 0f;


                if (unscaledDeltaTime > maxDeltaTime || maxTime + maxTimeout < Time.unscaledTime)
                {
                    maxDeltaTime = unscaledDeltaTime;
                    maxTime = Time.unscaledTime;
                }

                if (unscaledDeltaTime < minDeltaTime || minTime + maxTimeout < Time.unscaledTime)
                {
                    minDeltaTime = unscaledDeltaTime;
                    minTime = Time.unscaledTime;
                }



                frameIndex = 0;
                bufferIndex = (bufferIndex + 1) % bufferBuffer;
            }


            /*deltaTimes[frameIndex] = Time.deltaTime;
            unscaledDeltaTimes[frameIndex] = Time.unscaledDeltaTime;
            frameIndex = (frameIndex + 1) % frameBuffer;

            if (Time.unscaledDeltaTime < .1f && (Time.unscaledDeltaTime > maxDeltaTime || maxTime + maxTimeout < Time.unscaledTime))
            {
                maxDeltaTime = Time.unscaledDeltaTime;
                maxTime = Time.unscaledTime;
            }

            if (Time.unscaledDeltaTime < minDeltaTime || minTime + maxTimeout < Time.unscaledTime)
            {
                minDeltaTime = Time.unscaledDeltaTime;
                minTime = Time.unscaledTime;
            }*/

        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (Application.targetFrameRate == 60) Application.targetFrameRate = -1;
            else Application.targetFrameRate = 60;
        }

	}


}
