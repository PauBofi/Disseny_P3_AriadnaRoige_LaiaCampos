using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    Slider healthSlider;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        StartCoroutine(AnimateHealth(health));
    }

    IEnumerator AnimateHealth(int targetHealth)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        float startValue = healthSlider.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(startValue, targetHealth, elapsed / duration);
            yield return null;
        }

        healthSlider.value = targetHealth;
    }

}
