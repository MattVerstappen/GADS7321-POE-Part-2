using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed of the player
    [SerializeField] private float speed = 5f;
    // Jumping speed of the player
    [SerializeField] private float jumpSpeed = 10f;
    // Layer mask for detecting ground
    [SerializeField] private LayerMask groundLayer;
    // Range for interaction with objects
    [SerializeField] private float interactionRange = 2f;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private float groundCheckRadius = 0.5f;
    private Vector2 groundCheckOffset;
    private DialogueTrigger currentDialogueTrigger;

    // Called when the PlayerMovement object is created
    private void Awake()
    {
        InitializeComponents();
        groundCheckOffset = new Vector2(0, -GetComponent<Collider2D>().bounds.extents.y - 0.1f);
    }

    // Called every fixed frame
    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        CheckGrounded();
        HandleMovement();
        HandleJumping();
        HandleInteraction();
    }

    // Initializes required components
    private void InitializeComponents()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("Components Initialized");
    }

    // Handles player movement
    private void HandleMovement()
    {
        Vector2 moveInput = GetMoveInput();
        body.velocity = new Vector2(moveInput.x * speed, body.velocity.y);

        // Flip player when moving left and right
        if (moveInput.x > 0.01f)
            transform.localScale = Vector3.one;
        else if (moveInput.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Trigger animation based on movement
        anim.SetBool("run", Mathf.Abs(body.velocity.x) > 0.01f);
    }

    // Retrieves player movement input
    private Vector2 GetMoveInput()
    {
        return new Vector2(
            Input.GetAxisRaw("Horizontal"),
            0
        );
    }

    // Handles player jumping
    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    // Checks if the player is grounded
    private void CheckGrounded()
    {
        Vector2 position = (Vector2)transform.position + groundCheckOffset;
        grounded = Physics2D.OverlapCircle(position, groundCheckRadius, groundLayer);
        Debug.Log("Player grounded state: " + grounded);

        // Trigger animation based on grounded state
        anim.SetBool("grounded", grounded);
    }
    
    // Handles player interaction with objects
    private void HandleInteraction()
    {
        InputManager inputManager = InputManager.GetInstance();
        if (inputManager != null && inputManager.GetInteractPressed() && currentDialogueTrigger != null)
        {
            Debug.Log("Interact button pressed while in range.");
            // Invoke the TriggerDialogue method of the DialogueTrigger class
            currentDialogueTrigger.Invoke("TriggerDialogue", 0f);
        }
        
        if (inputManager != null && inputManager.GetSubmitPressed())
        {
            Debug.Log("Submit button pressed in PlayerMovement.");
        }
    }

    // Called when a collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D collider)
    {
        DialogueTrigger dialogueTrigger = collider.gameObject.GetComponent<DialogueTrigger>();
        if (dialogueTrigger != null)
        {
            currentDialogueTrigger = dialogueTrigger;
        }
    }

    // Called when a collider exits the trigger collider
    private void OnTriggerExit2D(Collider2D collider)
    {
        DialogueTrigger dialogueTrigger = collider.gameObject.GetComponent<DialogueTrigger>();
        if (dialogueTrigger != null && dialogueTrigger == currentDialogueTrigger)
        {
            currentDialogueTrigger = null;
        }
    }

    // Draws interaction range and ground check radius in the     // Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
        Vector2 position = (Vector2)transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(position, groundCheckRadius);
    }
}

