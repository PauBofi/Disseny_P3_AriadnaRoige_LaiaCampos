using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChestTpToNextScene : MonoBehaviour
{
    static int levelToGo = 2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        Debug.Log("Jugador tocó el cofre");
        SceneManager.LoadScene($"Level {levelToGo}");
        levelToGo += 1;
    }
}
