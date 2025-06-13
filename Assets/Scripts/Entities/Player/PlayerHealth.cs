using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    private int currentHealth;
    private int minHealth = 0;

    [Header("Sound")]
    public AudioClip soundHurt;

    public System.Action OnDeath;

    private Healthbar healthbar;

    public void Initialize(Healthbar healthbar)
    {
        this.healthbar = healthbar;
        currentHealth = maxHealth;
        this.healthbar.SetMaxHealth(maxHealth);
        this.healthbar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlaySFX(soundHurt);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= minHealth)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
