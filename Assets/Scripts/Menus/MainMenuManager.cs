using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;
    public Button exitButton;

    public GameObject settingsPanel;
    public Slider volumeSlider;

    public AudioClip soundButtons;
    public AudioClip musicMenu;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
       // settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);

        volumeSlider.onValueChanged.AddListener(SetVolume);

        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        AudioManager.Instance.PlayMusic(musicMenu);

        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        AudioManager.Instance.PlaySFX(soundButtons);        
        SceneManager.LoadScene("LevelSelector");
    }
    public void OpenSettings()
    {
        AudioManager.Instance.PlaySFX(soundButtons);
        if (settingsPanel.activeInHierarchy == false)
        {
            settingsPanel.SetActive(true);
        } 
        else if (settingsPanel.activeInHierarchy == true)
        {
            settingsPanel.SetActive(false);
        }            

        Debug.Log("Abrir ajustes");
    }
    public void ExitGame()
    {
        AudioManager.Instance.PlaySFX(soundButtons);
        Debug.Log("Salir del juego");
        Application.Quit();       
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();
    }
}
