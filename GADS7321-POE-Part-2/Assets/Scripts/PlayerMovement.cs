using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private float groundCheckRadius = 0.5f;
    private Vector2 groundCheckOffset;

    private void Awake()
    {
        InitializeComponents();
        groundCheckOffset = new Vector2(0, -GetComponent<Collider2D>().bounds.extents.y - 0.1f);
    }

    private void Update()
    {
        Debug.Log("Update method executed");
        HandleMovement();
        HandleJumping();
        UpdateAnimations();
    }

    private void InitializeComponents()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("Components Initialized");
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        // Flip player when moving left and right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJumping()
    {
        CheckGrounded();

        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    private void CheckGrounded()
    {
        Vector2 position = (Vector2)transform.position + groundCheckOffset;
        grounded = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
    }


    private void UpdateAnimations()
    {
        anim.SetBool("run", Math.Abs(body.velocity.x) > 0.01f);
        anim.SetBool("grounded", grounded);
    }
}
