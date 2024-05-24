using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 1f;
    [SerializeField] private LayerMask interactableLayer;

    private PlayerControls controls;

    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }
        instance = this;

        controls = new PlayerControls();

        // Subscribe to interaction input action
        controls.game.interact.performed += _ => InteractWithObjects();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void InteractWithObjects()
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

    public static InputManager GetInstance()
    {
        return instance;
    }
}