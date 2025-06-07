using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }
    
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }    
}
