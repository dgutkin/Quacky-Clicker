using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    public AudioSource efxSource;
    public AudioSource musicSource;

    public SpriteToggle[] Buttons;

    internal float currentState = 1.0f;
    internal bool musicOff = false;
    internal bool fxOff = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        efxSource.volume = PlayerPrefs.GetFloat("SOUND_ON", 1.0f);
        musicSource.volume = PlayerPrefs.GetFloat("MUSIC_ON", 0.3f);

        musicOff = System.Convert.ToBoolean(PlayerPrefs.GetInt("MUTE_MUSIC", 0));
        fxOff = System.Convert.ToBoolean(PlayerPrefs.GetInt("MUTE_FX", 0));
        SetSoundOnOff(musicOff, fxOff);
    }

    public void PlaySound(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlaySound2(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.pitch = Random.Range(0.9f, 1.1f);
        efxSource.Play();
    }

    public void ToggleSoundOnOff()
    {
        if (!musicOff & !fxOff)
        {
        	SetSoundOnOff(!musicOff, fxOff);
        } else if (musicOff & !fxOff) {
        	SetSoundOnOff(musicOff, !fxOff);
        } else {
        	SetSoundOnOff(!musicOff, !fxOff);
        }
    }

    public void SetSoundOnOff(bool music, bool fx)
    {
        musicOff = music;
        fxOff = fx;
     
        PlayerPrefs.SetInt("MUTE_MUSIC", musicOff ? 1 : 0);
        PlayerPrefs.SetInt("MUTE_FX", fxOff ? 1 : 0);

        efxSource.mute = fxOff;
        musicSource.mute = musicOff;

        foreach (SpriteToggle buttonToggle in Buttons)
            buttonToggle.SetToggleValue(System.Convert.ToInt32(!musicOff) + System.Convert.ToInt32(!fxOff) - 1);

    }

    public bool GetSoundEfxMute()
    {
        return !efxSource.mute;

    }
}
