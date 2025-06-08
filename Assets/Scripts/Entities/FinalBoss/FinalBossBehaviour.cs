using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossBehaviour : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float chaseRange = 10f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float attackCooldown = 3f;
    private bool canAttack = true;
    private bool isAttacking = false;

    public int maxHealth = 20;
    private int currentHealth;

    public PlayerChangeManagement playerChangeManagement;
    private Rigidbody2D rb;

    public AudioClip soundDeath;
    public AudioClip soundBullet;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        playerChangeManagement.OnPlayerChanged += RegisterNewPlayer;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && !isAttacking)
        {
            ChasePlayer();

            if (canAttack)
            {
                StartCoroutine(ShootProjectile());
            }
        }

        if (currentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX(soundDeath);
            Die();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    IEnumerator ShootProjectile()
    {
        canAttack = false;
        isAttacking = true;

        AudioManager.Instance.PlaySFX(soundBullet);

        Vector2 shootDir = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = shootDir * projectileSpeed;

        Destroy(proj, 10f);
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void Die()
    {
        SceneManager.LoadScene("YouWin");
        Destroy(gameObject);
    }

    void RegisterNewPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }

}
