using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 10f;
    public float maxReachableDistance = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    private float jumpForceToApply;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Grounded: " + isGrounded + ", Velocity: " + rb.velocity.x);

        isGrounded = rb.IsTouchingLayers(groundLayer);

        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        if (isGrounded)
        {
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 5f, groundLayer);

            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 5f, groundLayer);

            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, groundLayer);

            Debug.DrawRay(transform.position, new Vector2(direction, 1f).normalized * 4f, Color.green);

            if (!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
                jumpForceToApply = jumpForce;
            }
            else if (isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
                jumpForceToApply = jumpForce;
            }

            RaycastHit2D platformAhead = Physics2D.Raycast(transform.position, new Vector2(direction, 1f).normalized, 4f, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(direction, 1f).normalized * 4f, Color.green);

            if (platformAhead.collider != null)
            {
                float distance = Vector2.Distance(transform.position, platformAhead.point);
                if (distance <= maxReachableDistance)
                {
                    float adjustedForce = Mathf.Clamp(distance * 1.2f, 3f, jumpForce);
                    jumpForceToApply = adjustedForce;
                    shouldJump = true;
                }
            }

        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);
        }

        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 jumpDirection = direction * jumpForceToApply;

            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }


    }
}