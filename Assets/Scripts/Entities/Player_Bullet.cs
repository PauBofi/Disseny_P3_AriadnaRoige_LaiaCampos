using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    public float Speed;

    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        if (Rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D component not found on " + gameObject.name);
        }
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Rigidbody2D != null)
        {
            Rigidbody2D.velocity = Direction * Speed;
        }
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        Enemy enemy = collision.GetComponent<Enemy>();

        if (player != null)
        {
            player.Hit();
        }
        if (enemy != null)
        {
            enemy.Hit();
        }

        DestroyBullet();
    }
}
/*public class Player_Bullet : MonoBehaviour
{
    public float Speed;

    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player_Movimiento player = collision.GetComponent<Player_Movimiento>();
        Enemy enemy = collision.GetComponent<Enemy>();

        if (player != null)
        {
            player.Hit();
        }
        if (enemy != null)
        {
            enemy.Hit();
        }

        DestroyBullet();
    }
}*/
