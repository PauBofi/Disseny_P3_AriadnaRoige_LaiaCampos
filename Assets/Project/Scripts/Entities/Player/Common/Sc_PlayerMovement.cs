using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    public AudioClip soundRun;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Animator")]
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Check ground, gravity, wall, and update physics 
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        //If he is not walljumping, move and turn normally
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            Flip();
        }

        //Animations
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        //Movimiento físico
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    //Salto normal y walljump
    public void Jump(InputAction.CallbackContext context)
    {
        //Salto normal
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;
                smokeFX.Play();
                animator.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
                smokeFX.Play();
                animator.SetTrigger("jump");
            }
        }

        //Salto en pared
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;
            smokeFX.Play();
            animator.SetTrigger("jump");

            //Forzar flip si está mirando al revés
            if (transform.localScale.x != wallJumpDirection)
            {
                Flip();
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }

    //Verifica si toca el suelo
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    //Verifica si toca una pared
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    //Gestiona gravedad y límite de caída
    private void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    //Gestiona el deslizamiento en la pared
    private void ProcessWallSlide()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    //Gestiona salto en pared
    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    //Cancela el walljump
    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    //Lee el movimiento horizontal
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        AudioManager.Instance.PlaySFX(soundRun);
    }

    //Gira el personaje
    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if (rb.velocity.y == 0)
            {
                smokeFX.Play();
            }
        }
    }

    //Dibuja gizmos de debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
