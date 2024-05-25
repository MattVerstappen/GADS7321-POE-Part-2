using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;

    private PlayerControls controls;

    private static InputManager instance;

    private bool interactPressed = false;
    private bool submitPressed = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;

        controls = new PlayerControls();

        // Subscribe to interaction input action
        controls.game.interact.performed += _ => interactPressed = true;
        controls.game.submit.performed += context => SubmitPressed(context);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void InteractWithObjects()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("Interacted with: " + interactable.name);
            }
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
            Debug.Log("Submit button pressed");
        }
        else if (context.canceled)
        {
            submitPressed = false;
            Debug.Log("Submit button released");
        }
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }
    
    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }
}
