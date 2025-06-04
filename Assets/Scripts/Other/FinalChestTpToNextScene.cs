using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChestTpToNextScene : MonoBehaviour
{
    static int levelToGo = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        Debug.Log("Jugador tocó el cofre");
        levelToGo += 1;
        SceneManager.LoadScene($"Level {levelToGo}");
    }
}
