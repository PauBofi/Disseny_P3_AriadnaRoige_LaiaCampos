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

        //Configurar la barra de vida si existe
        if (bossHealthBar != null)
        {
            bossHealthBar.maxValue = maxHealth;
            bossHealthBar.value = maxHealth;
        }

        //Suscribirse al evento de cambio de jugador
        playerChangeManagement.OnPlayerChanged += RegisterNewPlayer;
    }

    void Update()
    {
        //Si no hay player, no hacer nada
        if (player == null) return;

        //Revisar si debe girarse hacia el jugador
        HandleFlip();

        //Calcular distancia entre este y el jugador
        float distance = Vector2.Distance(transform.position, player.position);

        //Si está dentro del rango de persecución y no atacando, persigue al player
        if (distance <= chaseRange && !isAttacking)
        {
            ChasePlayer();

            //Si puede atacar y está dentro del rango de disparo, dispara
            if (canAttack && distance <= shootRange)
            {
                StartCoroutine(ShootProjectile());
            }
        }

        //Si la vida llega a 0, se gestiona la muerte con la funcion Die()
        if (currentHealth <= 0)
        {
            AudioManager.Instance.PlaySFX(soundDeath);
            Die();
        }

        Debug.DrawRay(transform.position, Vector2.left * 30f, Color.magenta);
    }

    //Se mueve hacia el jugador
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    //Dispara el proyectil con cooldown
    IEnumerator ShootProjectile()
    {
        canAttack = false;
        isAttacking = true;

        AudioManager.Instance.PlaySFX(soundBullet);

        //Calcula la dirección hacia el jugador
        Vector2 shootDir = (player.position - transform.position).normalized;

        //Instancia y lanza el proyectil
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = shootDir * projectileSpeed;

        //Destruir proyectil despues de  10 segundos
        Destroy(proj, 10f);

        isAttacking = false;

        //Espera antes de poder atacar de nuevo
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    //Recibe daño y actualiza la barra de vida
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (bossHealthBar != null)
        {
            bossHealthBar.value = currentHealth;
        }
    }

    //Muere y cambia a la escena de victoria
    void Die()
    {
        SceneManager.LoadScene("S_YouWin");
        Destroy(gameObject);
    }

    //Registra el nuevo jugador cuando cambia de personaje
    void RegisterNewPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }

    //Verifica si debe girarse para mirar al jugador
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

    //Se gira si llaman a esta funcion
    void Flip()
    {
        isFacingLeft = !isFacingLeft;
        Vector3 ls = transform.localScale;
        ls.x *= -1;
        transform.localScale = ls;
    }

}
