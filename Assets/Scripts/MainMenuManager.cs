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

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitGame);

        volumeSlider.onValueChanged.AddListener(SetVolume);

        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void StartGame()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    void OpenSettings()
    {
        Debug.Log("Abrir ajustes");
    }
    void ExitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();       
    }
    void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();
    }
}
