using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralAudio : MonoBehaviour
{
    public AudioSource musicSource;   
    public AudioSource sfxSource;

    void Start()
    {
        AudioManager.Instance.PlayMusic(musicaMenu, true);
        AudioManager.Instance.PlaySFX(sfxSource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
