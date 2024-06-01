using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    
    [SerializeField] private DialogueManager dialogueManager;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }
    
    

    private void Update()
    {
        if (playerInRange && InputManager.GetInstance().GetInteractPressed())
        {
            if (!DialogueManager.GetInstance().dialogueIsPlaying)
            { 
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            visualCue.SetActive(false);
        }
    }
}
