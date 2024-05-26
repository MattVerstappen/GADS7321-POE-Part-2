using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();
    public GameObject questPanel;
    public void AddQuest(Quest quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            UpdateQuestUI();
        }
    }
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
        // Implement logic to update the quest UI to reflect changes in the active quests list, update the quest panel to display the current list of active quests
    }
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