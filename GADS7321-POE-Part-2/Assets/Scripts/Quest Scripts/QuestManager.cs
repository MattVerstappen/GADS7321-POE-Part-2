using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();
    public GameObject questPanel; // Reference to the QuestPanel GameObject

    // Method to add a new quest to the active quests list
    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            // Optionally, update the quest UI to display the new quest
            UpdateQuestUI();
        }
    }

    // Method to remove a quest from the active quests list
    public void RemoveQuest(Quest quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            // Optionally, update the quest UI to reflect the removed quest
            UpdateQuestUI();
        }
    }

    // Method to update the quest UI (if applicable)
    private void UpdateQuestUI()
    {
        // Implement logic to update the quest UI to reflect changes in the active quests list
        // For example, update the quest panel to display the current list of active quests
    }

    // Method to toggle the visibility of the quest panel
    private void ToggleQuestUI()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(!questPanel.activeSelf);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleQuestUI();
        }
    }
}