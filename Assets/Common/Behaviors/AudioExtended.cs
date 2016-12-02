using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Audio/Audio Extended")]
public class AudioExtended : MonoBehaviour
{
    public RandomizedFloat pitch = -1f;
    public RandomizedFloat volume = -1f;
    public RandomizedFloat delay = 0f;

    private bool loop = false;
    public bool varyOnLoop = true;

    public bool ignoreListenerVolume = false;
    public bool ignoreListenerPause = false;

    private AudioSource _audioSource;
    public AudioSource audioSource
    {
        get
        {
            if(_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            return _audioSource;
        }
    }

    private bool delaying = false;

    void Awake()
    {

        audioSource.ignoreListenerVolume = ignoreListenerVolume;
        audioSource.ignoreListenerPause = ignoreListenerPause;

        if (pitch == -1f)
        {
            pitch = audioSource.pitch;
        }
        if (volume == -1f)
        {
            volume = audioSource.volume;
        }
    }

    void Start()
    {
        //Vary();

        if (audioSource.playOnAwake)
        {
            audioSource.Stop();
            Play();
        }
    }

    /*void OnEnable()
    {
        if (audioSource.playOnAwake || audioSource.isPlaying)
        {
            audioSource.Stop();
            Play();
        }
    }*/

    void Update()
    {
        if (delaying && audioSource.isPlaying)
            delaying = false;

        if (loop && !audioSource.loop && !audioSource.isPlaying && !delaying)
            PlayVaried();
    }

    public void Play()
    {
        if (audioSource.loop)
        {
            loop = true;

            if (varyOnLoop)
            {
                audioSource.loop = false;
            }
        }

        PlayVaried();
    }

    void Vary()
    {
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        //audioSource.volume *= pitch.Center() / audioSource.pitch;
    }

    void PlayVaried()
    {
        Vary();
        //audioSource.PlayScheduled(AudioSettings.dspTime + delay);
        float thisDelay = Mathf.Max(0f, delay);

        if (thisDelay == 0f)
        {
            audioSource.Play();
            delaying = false;
        }
        else
        {
            audioSource.PlayDelayed(thisDelay);
            delaying = true;
        }

        //if(delay > 0f && audioSource.isPlaying)
    }

}
