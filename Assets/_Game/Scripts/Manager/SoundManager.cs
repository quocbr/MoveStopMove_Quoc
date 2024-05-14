using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<AudioClip> listMusicSound; 
    [SerializeField] private List<AudioClip> listSFXSound;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sFXSource;

    public void PlayMusic(string name)
    {
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
}
