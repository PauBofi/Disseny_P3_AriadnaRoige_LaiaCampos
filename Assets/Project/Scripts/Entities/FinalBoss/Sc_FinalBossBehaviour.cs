using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FinalBossBehaviour : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float chaseRange = 30f;
    public float shootRange = 15f;

    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float attackCooldown = 5f;
    private bool canAttack = true;
    private bool isAttacking = false;

    public int maxHealth = 20;
    private int currentHealth;

    private bool isFacingLeft = true;

    [SerializeField] private Slider bossHealthBar;

    public PlayerChangeManagement playerChangeManagement;
    private Rigidbody2D rb;

    public AudioClip soundDeath;
    public AudioClip soundBullet;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxHealth;
            bossHealthBar.value = maxHealth;
        }
        playerChangeManagement.OnPlayerChanged += RegisterNewPlayer;
    }

    void Update()
    {
        if (player == null) return;

        HandleFlip();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && !isAttacking)
        {
            ChasePlayer();

            if (canAttack && distance <= shootRange)
            {
                StartCoroutine(ShootProjectile());
            }
        }

        if (currentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX(soundDeath);
            Die();
        }

        Debug.DrawRay(transform.position, Vector2.left * 30f, Color.magenta);
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
        
        if (bossHealthBar != null)
        {
            bossHealthBar.value = currentHealth;
        }
    }

        void Die()
    {
        SceneManager.LoadScene("S_YouWin");
        Destroy(gameObject);
    }

    void RegisterNewPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }

    void HandleFlip()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x && isFacingLeft)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && !isFacingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 ls = transform.localScale;
        ls.x *= -1;
        transform.localScale = ls;
    }

}
