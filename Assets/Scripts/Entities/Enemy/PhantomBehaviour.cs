using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    private bool canShoot = true;
    public float shootCooldown = 1f;
    private float timeToDestroy = 5f;

    public Transform player;
    public float chaseSpeed = 2f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private int directionX = 1;

    int maxHealth = 5;
    int currentHealth;
    int minHealth = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float raycastLength = 6f;
        Vector2[] directions =
        {
            Vector2.right,
            Vector2.left,
            Vector2.up,
            (Vector2.down + Vector2.right).normalized,
            (Vector2.up + Vector2.right).normalized,
            (Vector2.up + Vector2.left).normalized
        };

        foreach (Vector2 d in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, d, raycastLength, playerLayer);
            Debug.DrawRay(transform.position, d * raycastLength, Color.red);

            if (hit.collider != null && hit.collider.transform == player)
            {
                Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
                Shoot(direction);
                break;
            }
        }

        if (currentHealth <= minHealth)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D groundAhead = Physics2D.Raycast(transform.position + new Vector3(directionX * 0.5f, 0, 0), Vector2.down, 5f, groundLayer);
        Debug.DrawRay(transform.position + new Vector3(directionX * 0.5f, 0, 0), Vector2.down * 2f, Color.blue);

        if (!groundAhead.collider)
        {
            directionX *= -1;
        }
        if (canShoot)
        {
            rb.velocity = new Vector2(directionX * chaseSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); 
        }
    }

    void Shoot(Vector2 direction)
    {
        if (!canShoot) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        canShoot = false;

        Invoke(nameof(ResetShoot), shootCooldown);

        Destroy(bullet, timeToDestroy);
    }

    void ResetShoot()
    {
        canShoot = true;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
