using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalChestTpToNextScene : MonoBehaviour
{
    static int levelToGo = 0;
    public AudioClip sonidoFinal;
    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.Instance.PlaySFX(sonidoFinal);
        Destroy(other.gameObject);
        Debug.Log("Jugador toc� el cofre");
        levelToGo += 1;
        SceneManager.LoadScene($"Level {levelToGo}");
    }
}
