using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject phantomPrefab;
    public float phantomSpawnCooldown = 10f;
    private bool canSpawn = true;

    public int maxHealth = 20;
    private int currentHealth;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
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

        if (canSpawn)
        {
            StartCoroutine(SpawnPhantom());
        }

        if (currentHealth <= 0)
        {
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

        Vector2 shootDir = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = shootDir * projectileSpeed;

        Destroy(proj, 5f);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        isAttacking = false;
    }

    IEnumerator SpawnPhantom()
    {
        canSpawn = false;

        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-2f, 2f), 0, 0);
        Instantiate(phantomPrefab, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(phantomSpawnCooldown);
        canSpawn = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
