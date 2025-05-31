using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChestTpToNextScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Jugador tocó el cofre");
        SceneManager.LoadScene("Level 2");
    }
}
