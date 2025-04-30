using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject Player;

    private float LastShoot;
    private int Health = 3;

    private void Update()
    {
        // Ensure Player is assigned
        if (Player == null)
        {
            Debug.LogError("Player is not assigned in the inspector for " + gameObject.name);
            return;
        }

        Vector3 direction = Player.transform.position - transform.position;
        if (direction.x >= 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        float distance = Mathf.Abs(Player.transform.position.x - transform.position.x);

        // Adjust the distance check to ensure the enemy shoots when the player is within range
        if (distance < 5.0f && Time.time > LastShoot + 0.7f) // Increased the cooldown to 1 second
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        Debug.Log("Enemy Shoot method called");

        // Ensure BulletPrefab is assigned
        if (BulletPrefab == null)
        {
            Debug.LogError("BulletPrefab is not assigned in the inspector for " + gameObject.name);
            return;
        }

        Vector3 direction;
        if (transform.localScale.x == 1.0f)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        Vector3 spawnPosition = transform.position + direction * 0.5f;

        GameObject Bullet = Instantiate(BulletPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Enemy Bullet instantiated at position: " + spawnPosition);

        Player_Bullet bulletComponent = Bullet.GetComponent<Player_Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(direction);
            Debug.Log("Bullet direction set to: " + direction);
        }
        else
        {
            Debug.LogError("Player_Bullet component not found on BulletPrefab");
        }
    }

    //Vida
    public void Hit()
    {
        Health = Health - 1;
        if (Health == 0)
        {
            Destroy(gameObject);
        }
    }
}
/*public class Enemy : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject Player;

    private float LastShoot;
    private int Health = 3;

    private void Update()
    {
        Vector3 direction = Player.transform.position - transform.position;
        if (direction.x >= 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        float distance = Mathf.Abs(Player.transform.position.x - transform.position.x);

        if (distance < 1.0f && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        GameObject Bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        Bullet.GetComponent<Player_Bullet>().SetDirection(direction);
    }

    //Vida
    public void Hit()
    {
        Health = Health - 1;
        if (Health == 0)
        {
            Destroy(gameObject);
        }
    }
}*/
