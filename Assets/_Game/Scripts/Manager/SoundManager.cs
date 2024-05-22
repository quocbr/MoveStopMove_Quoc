using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SettingSO settingSO;
    [SerializeField] private List<AudioClip> listMusicSound; 
    [SerializeField] private List<AudioClip> listSFXSound;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sFXSource;

    public bool isMute
    {
        get { return settingSO.isMuteMusicAndSound; }
        set
        {
            settingSO.isMuteMusicAndSound = value;
        }
    }

    public void Awake()
    {
        musicSource.mute = settingSO.isMuteMusicAndSound;
        sFXSource.mute = settingSO.isMuteMusicAndSound;
    }

    public void PlayMusic(string name,bool isLoop = false)
    {
        musicSource.loop = isLoop;

        AudioClip clip = null;
        for (int i = 0; i < listMusicSound.Count; i++)
        {
            if (listMusicSound[i].name == name)
            {
                clip = listMusicSound[i];
                break;
            }
        }

        if(clip == null)
        {
            Debug.Log("Error Sound Music");
        }
        else
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource.clip != null)
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
        musicSource.loop = false;
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = null;
        for (int i = 0; i < listSFXSound.Count; i++)
        {
            if (listSFXSound[i].name == name)
            {
                clip = listSFXSound[i];
                break;
            }
        }

        if (clip == null)
        {
            Debug.Log("Error Sound SFX");
        }
        else
        {
            sFXSource.clip = clip;
            sFXSource.Play();
        }
    }

    public AudioClip GetSFX(string name)
    {
        AudioClip clip = null;
        for (int i = 0; i < listSFXSound.Count; i++)
        {
            if (listSFXSound[i].name == name)
            {
                clip = listSFXSound[i];
                break;
            }
        }

        return clip;
    }

    [ContextMenu("on")]
    public void SoundFXOn()
    {
        settingSO.isMuteMusicAndSound = false;
        musicSource.mute = false;
        sFXSource.mute = false;
    }
    [ContextMenu("of")]
    public void SoundFXOff()
    {
        settingSO.isMuteMusicAndSound = true;
        musicSource.mute = true;
        sFXSource.mute = true;
    }
}
