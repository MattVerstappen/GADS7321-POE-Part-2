using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private PlayerControls controls;

    private Vector2 moveDirection = Vector2.zero;
    private bool jumpPressed = false;
    private bool interactPressed = false;
    private bool submitPressed = false;

    // Called when the InputManager object is created
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;
        controls = new PlayerControls();
        
        // Register callback for the "interact" action - > for some reason not including this prevents interaction from happening
        controls.game.interact.performed += InteractButtonPressed;
        controls.game.interact.canceled += InteractButtonPressed;

        // Register callback for the "submit" action
        controls.game.submit.performed += SubmitPressed;
        controls.game.submit.canceled += SubmitPressed;


        Debug.Log("InputManager instance set.");

        // Enable input controls
        controls.Enable();
    }

    // Called when the InputManager object is disabled
    private void OnDisable()
    {
        // Disable input controls
        controls.Disable();
    }

    // Retrieves the InputManager instance
    public static InputManager GetInstance()
    {
        return instance;
    }

    // Handles player movement input
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    // Handles player jump input
    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
        else if (context.canceled)
        {
            jumpPressed = false;
        }
    }

    // Handles player interact input
    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }

    // Handles player submit input
    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    // Retrieves the player's movement direction
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    // Retrieves the state of the jump button
    public bool GetJumpPressed()
    {
        bool result = jumpPressed;
        jumpPressed = false;
        return result;
    }

    // Retrieves the state of the interact button
    public bool GetInteractPressed()
    {
        return interactPressed;
    }

    // Retrieves the state of the submit button
    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    // Registers the submit button press
    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
}
