using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Method called when the player interacts with the object
    public void Interact()
    {
        // Add your interaction logic here
        Debug.Log("Interaction logic executed for: " + gameObject.name);
    }
}