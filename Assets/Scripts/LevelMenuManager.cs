using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    public Button buttonLevel0;
    public Button buttonLevel1;
    public Button buttonLevel2;
    public Button returnMainMenu;
    public Button levelButtonSettings;

    public GameObject settingsPanel;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        buttonLevel0.onClick.AddListener(ButtonLevel0);
        buttonLevel1.onClick.AddListener(ButtonLevel1);
        buttonLevel2.onClick.AddListener(ButtonLevel2);
        returnMainMenu.onClick.AddListener(ReturnMainMenu);
        levelButtonSettings.onClick.AddListener(LevelButtonSettings);

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
    void ButtonLevel0()
    {
        SceneManager.LoadScene("Level 1 Nina");
    }
    void ButtonLevel1()
    {
        SceneManager.LoadScene("Level 1 Nina");
    }
    void ButtonLevel2()
    {
        SceneManager.LoadScene("Level 1 Nina");
    }
    void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void LevelButtonSettings()
    {
        Debug.Log("Abrir ajustes");
    }
    void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();
    }
}
