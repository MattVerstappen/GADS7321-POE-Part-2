using UnityEngine;
using UnityEngine.InputSystem;

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
        CheckGrounded(); // Ensure grounded status is updated continuously
        HandleJumping();
        HandleMovement();
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
        Vector2 moveInput = GetMoveInput();
        body.velocity = new Vector2(moveInput.x * speed, body.velocity.y);

        // Flip player when moving left and right
        if (moveInput.x > 0.01f)
            transform.localScale = Vector3.one;
        else if (moveInput.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private Vector2 GetMoveInput()
    {
        return Keyboard.current == null ? Vector2.zero : new Vector2(
            Keyboard.current.dKey.isPressed ? 1 : Keyboard.current.aKey.isPressed ? -1 : 0,
            0
        );
    }

    private void HandleJumping()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("run", Mathf.Abs(body.velocity.x) > 0.01f);
        anim.SetBool("grounded", grounded);
    }

    private void CheckGrounded()
    {
        Vector2 position = (Vector2)transform.position + groundCheckOffset;
        grounded = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 position = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(position, groundCheckRadius);
    }
}
