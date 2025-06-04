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
    public Button buttonLevelBoss;
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
        buttonLevelBoss.onClick.AddListener(ButtonLevelBoss);
        returnMainMenu.onClick.AddListener(ReturnMainMenu);
        //levelButtonSettings.onClick.AddListener(LevelButtonSettings);

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
    public void ButtonLevel0()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void ButtonLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void ButtonLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void ButtonLevelBoss()
    {
        SceneManager.LoadScene("Level 3");
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LevelButtonSettings()
    {
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
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume);
        PlayerPrefs.Save();
    }
}
