using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    static int levelToGo = 0;
    public AudioClip sonidoFinal;
    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.Instance.PlaySFX(sonidoFinal);
        Destroy(other.gameObject);
        levelToGo += 1;
        SceneManager.LoadScene($"Level{levelToGo}");
    }
}
