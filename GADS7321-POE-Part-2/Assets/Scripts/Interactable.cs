using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("Interaction logic executed for: " + gameObject.name);
    }
}