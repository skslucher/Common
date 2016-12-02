using UnityEngine;
using System.Collections;

public class GameSettings : Singleton<GameSettings> {

    public static bool screenEffectsEnabled = true;
    

    public static bool GetBool(string field)
    {
        switch (field)
        {
            case "Game Volume Enabled":
                return AudioVolume.Game.enabled;
            case "Music Volume Enabled":
                return AudioVolume.Music.enabled;
            case "Screen Effects Enabled":
                return screenEffectsEnabled;
        }

        return false;
    }

    public static void SetBool(string field, bool newValue)
    {
        switch (field)
        {
            case "Game Volume Enabled":
                AudioVolume.Game.enabled = newValue;
                break;
            case "Music Volume Enabled":
                AudioVolume.Music.enabled = newValue;
                break;
            case "Screen Effects Enabled":
                screenEffectsEnabled = newValue;
                break;
        }
    }

    public static float GetFloat(string field)
    {
        switch (field)
        {
            case "Game Volume":
                return AudioVolume.Game.VolumeUnmodified;
            case "Music Volume":
                return AudioVolume.Music.VolumeUnmodified;
        }

        return 0f;
    }

    public static void SetFloat(string field, float value)
    {
        switch (field)
        {
            case "Game Volume":
                AudioVolume.Game.VolumeUnmodified = value;
                break;
            case "Music Volume":
                AudioVolume.Music.VolumeUnmodified = value;
                break;
        }
    }

    public static void Load()
    {
        AudioVolume.Game.VolumeUnmodified = PlayerPrefs.GetFloat("Game Volume", AudioVolume.Game.VolumeUnmodified);
        AudioVolume.Music.VolumeUnmodified = PlayerPrefs.GetFloat("Music Volume", AudioVolume.Music.VolumeUnmodified);
        
        screenEffectsEnabled = LoadBool("Screen Effects Enabled", screenEffectsEnabled);

    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("Game Volume", AudioVolume.Game.VolumeUnmodified);
        PlayerPrefs.SetFloat("Music Volume", AudioVolume.Music.VolumeUnmodified);
        
        SaveBool("Screen Effects Enabled", screenEffectsEnabled);

        PlayerPrefs.Save();

    }



    public static void SaveBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, Com.BoolToInt(value));
    }

    public static bool LoadBool(string key, bool defaultValue = false)
    {
        return Com.IntToBool(PlayerPrefs.GetInt(key, Com.BoolToInt(defaultValue)));
    }




    void Start()
    {
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
    }
}
