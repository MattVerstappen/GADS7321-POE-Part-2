using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 1.5f; // Interaction radius around the player
    [SerializeField] private LayerMask interactableLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObjects();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the interaction radius in the scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    private void InteractWithObjects()
    {
        // Find all colliders within the interaction radius around the player
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        // Loop through all detected colliders
        foreach (Collider2D collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                // Interact with the object
                interactable.Interact();

                // Display interaction feedback in the console
                Debug.Log("Interacted with: " + interactable.name);
            }
        }
    }
}