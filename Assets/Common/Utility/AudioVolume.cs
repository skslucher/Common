using UnityEngine;
using System.Collections;

public class AudioVolume : Singleton<AudioVolume>
{

    [System.Serializable]
    public class VolumeModule
    {
        public bool enabled = true;

        public float volume = 1f;
        
        private float modifier = 1f;
        

        public float Volume
        {
            get
            {
                return VolumeUnmodified * modifier;
            }
            set
            {
                VolumeUnmodified = value;
            }
        }

        public float VolumeUnmodified
        {
            get
            {
                if (enabled)
                {
                    return volume;
                }
                else
                {
                    return 0f;
                }
            }
            set
            {
                value = Mathf.Clamp01(value);

                if (value > 0)
                {
                    enabled = true;
                    volume = value;
                }
                else
                {
                    enabled = false;
                }
            }
        }


        public VolumeModule()
        {
            Volume = volume;
            modifier = 1f;
        }

        public VolumeModule(float volume)
        {
            Volume = volume;
            modifier = 1f;
        }


        public void FadeOut(float time, bool useRealTime = true)
        {
            AudioVolume.Instance.StartCoroutine(Fade(1f, 0f, time, useRealTime));
        }

        public void FadeIn(float time, bool useRealTime = true)
        {
            AudioVolume.Instance.StartCoroutine(Fade(0f, 1f, time, useRealTime));
        }

        public IEnumerator Fade(float startValue, float endValue, float time, bool useRealTime = true)
        {
            float startTime = GetTime(useRealTime);
            float endTime = startTime + time;

            do
            {
                yield return new WaitForEndOfFrame();
                modifier = Mathf.Lerp(startValue, endValue, Mathf.InverseLerp(startTime, endTime, GetTime(useRealTime)));

            } while (GetTime(useRealTime) < endTime);
        }

        private float GetTime(bool useRealTime = true)
        {
            if (useRealTime) return Time.unscaledTime;
            else return Time.time;
        }

    }

    public VolumeModule master = new VolumeModule(1f);
    public static VolumeModule Master { get { return Instance.master; } }

    public VolumeModule game = new VolumeModule(.8f);
    public static VolumeModule Game { get { return Instance.game; } }

    public VolumeModule music = new VolumeModule(.45f);
    public static VolumeModule Music { get { return Instance.music; } }


    public static void FadeOutGameVolume(float time, bool useRealtime = true)
    {
        Game.FadeOut(time, useRealtime);
    }

    public static void FadeInGameVolume(float time, bool useRealtime = true)
    {
        Game.FadeIn(time, useRealtime);
    }



    Transform parent;
    public AudioSource[] musicSources;

    void Reset()
    {
        master = new VolumeModule(1f);
        game = new VolumeModule(.8f);
        music = new VolumeModule(.45f);
    }

    void Awake()
    {
        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].ignoreListenerVolume = true;
            musicSources[i].ignoreListenerPause = true;
        }

    }
    
    void Update()
    {
        if (parent == null)
        {
            GameObject parentObject = GameObject.FindWithTag("MainCamera");
            if (parentObject != null)
            {
                parent = parentObject.transform;
            }
        }

        if (parent != null)
        {
            transform.position = parent.position;
        }

        UpdateVolumes();
    }


    public void UpdateVolumes()
    {
        AudioListener.volume = master.Volume * game.Volume;

        for (int i = 0; i < musicSources.Length; i++)
        {
            musicSources[i].volume = master.Volume * music.Volume;
        }
    }




}
