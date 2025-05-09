using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player_Movimiento : MonoBehaviour
{
    [SerializeField ]Healthbar healthbar;
    [SerializeField] Manabar manabar;

    public GameObject BulletPrefab;
    public float Speed;
    public float JumpForce;
    public Transform groundPosition; //Per veure i moure la posició del RayCast 

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;
    private int Health = 10;
    private int Mana = 30;

    public Transform wallPosition;
    private bool Walled;


    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animator.SetBool("jumpling", Grounded != true);
        Animator.SetBool("jumpling", Walled != true);


        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        Animator.SetBool("running", Horizontal != 0.0f);

        //RayCast per a que no faci modo floopyBird
        Debug.DrawRay(groundPosition.position, Vector3.down * 0.4f, Color.red);
        if (Physics2D.Raycast(groundPosition.position, Vector3.down, 0.4f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }

        Debug.DrawRay(wallPosition.position, Vector3.right * 0.4f, Color.red);
        if (Physics2D.Raycast(wallPosition.position, Vector3.right, 0.4f))
        {
            Walled = true;
        }
        else
        {
            Walled = false;
        }

    }
    
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        Animator.SetBool("jumpling", Grounded);
    }

    private void Shoot()
    {
        Debug.Log("Shoot method called");
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
        Debug.Log("Bullet instantiated at position: " + spawnPosition);

        Bullet.GetComponent<Player_Bullet>().SetDirection(direction);

        Mana = Mana - 1;
        manabar.SetMana(Mana);

    }

    private void FixedUpdate()
    {
        //Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    // Per veure el RayCast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundPosition.position, Vector2.down * 0.4f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallPosition.position, Vector2.down * 0.4f);
    }

    //Vida
    public void Hit()
    {
        Health = Health - 1;
        healthbar.SetHealth(Health);

        if (Health == 0)
        {
            Debug.Log("Muerto");
        }
    }
}
/*public class Player_Movimiento : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float Speed;
    public float JumpForce;
    public Transform groundPosition; //Per veure i moure la posició del RayCast 

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;
    private int Health = 10;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animator.SetBool("jumpling", Grounded != true);

        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        } 
        else if (Horizontal > 0.0f) 
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        Animator.SetBool("running", Horizontal != 0.0f);

        //RayCast per a que no faci modo floopyBird
        Debug.DrawRay(groundPosition.position, Vector3.down * 0.4f, Color.red);
        if (Physics2D.Raycast(groundPosition.position, Vector3.down, 0.4f))
        {
            Grounded = true;
        }
        else 
        { 
            Grounded = false;            
        }

        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();            
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
        Animator.SetBool("jumpling", Grounded);
    }

    private void Shoot ()
    {
        Vector3 direction;

        if (transform.localScale.x == 1.0f)
        {
            direction = Vector2.right;
        } else
        {
            direction = Vector2.left;
        }

        GameObject Bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        Bullet.GetComponent<Player_Bullet>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        //Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Rigidbody2D.linearVelocity.y);
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    // Per veure el RayCast
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundPosition.position, Vector2.down * 0.4f);
    }

    //Vida
    public void Hit()
    {
        Health = Health - 1;
        if (Health == 0)
        {
            Debug.Log("Muerto");
        }
    }
}*/
