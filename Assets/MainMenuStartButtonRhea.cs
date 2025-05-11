    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuStartButtonRhea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1 Rhea"); // Changes the main menu scene into the Level1 scene
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
