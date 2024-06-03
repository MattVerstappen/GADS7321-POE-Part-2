using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkills", menuName = "ScriptableObjects/PlayerSkills", order = 1)]
public class PlayerSkills : ScriptableObject
{
    [SerializeField]public bool hasLearnedSocialSkills = false;
    [SerializeField]public bool hasLearnedMindfulness = false;
    [SerializeField]public bool hasLearnedSelfAwareness = false;
    [SerializeField]public bool hasLearnedStressManagement = false;
    
    public void ResetSkills()
    {
        hasLearnedSocialSkills = false;
        hasLearnedMindfulness = false;
        hasLearnedSelfAwareness = false;
        hasLearnedStressManagement = false;
    }
}