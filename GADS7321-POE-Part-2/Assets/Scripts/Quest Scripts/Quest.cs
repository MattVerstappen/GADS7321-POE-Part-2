using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questID;
    public string title;
    public string description;
    public bool isCompleted;

    // Method to start the quest
    public virtual void StartQuest()
    {
        Debug.Log("Quest started: " + title);
    }

    // Method to complete the quest
    public virtual void CompleteQuest()
    {
        Debug.Log("Quest completed: " + title);
        isCompleted = true;
    }
}