using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    [Range(0.5f, 2.5f)]
    private Rigidbody2D rb;
    private int directionX = 1;

    public int maxHealth = 10;
    private int currentHealth;
    private int minHealth = 0;
    public float raycastLength = 6f;
    public float raycastGroundAheadLenght = 5f;
    public float raycastWallAheadLenght = 2f;

    public AudioClip soundDeath;
    public AudioClip soundBullet;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        //Se crea una array con las direcciones de los 6 raycast que detectaran al jugador
        Vector2[] directions =
        {
            Vector2.right,
            Vector2.left,
            Vector2.up,
            (Vector2.down + Vector2.right).normalized,
            (Vector2.up + Vector2.right).normalized,
            (Vector2.up + Vector2.left).normalized
        };

        //Recorre la array y crea un raycast por cada direccion. Usando el raycastLenght que es publico
        foreach (Vector2 d in directions)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, d, raycastLength, playerLayer);
            Debug.DrawRay(transform.position, d * raycastLength, Color.red);

            //Si en algun frame alguno de estos raycast detecta algo y es el player, se le asigna una direccion hacia el player y dispara
            if (hit.collider != null && hit.collider.transform == player)
            {
                Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized;
                Shoot(direction);
                break;
            }
        }

        //Gestor de muerte basico
        if (currentHealth <= minHealth)
        {
            AudioManager.Instance.PlaySFX(soundDeath);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        //Raycast proyactado hacia abajo delante del jugador para ver si hay suelo o no
        RaycastHit2D groundAhead = Physics2D.Raycast(transform.position + new Vector3(directionX * 0.5f, 0, 0), Vector2.down, raycastGroundAheadLenght, groundLayer);
        Debug.DrawRay(transform.position + new Vector3(directionX * 0.5f, 0, 0), Vector2.down * raycastGroundAheadLenght, Color.blue);

        //Raycast proyectado recto hacia la direccion del fantasma para ver si hay una pared
        RaycastHit2D wallAhead = Physics2D.Raycast(transform.position, Vector2.right * directionX , raycastWallAheadLenght, wallLayer);
        Debug.DrawRay(transform.position , Vector2.right * directionX * raycastWallAheadLenght, Color.magenta);

        //Si no se detecta suelo o se detecta una pared delante, se gira el fantasma
        if (!groundAhead.collider || wallAhead.collider)
        {
            directionX *= -1;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
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
        AudioManager.Instance.PlaySFX(soundBullet);
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
