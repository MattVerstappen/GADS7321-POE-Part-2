using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Response UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choiceText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; } = false;
    private bool responseSelected = false;
    private bool canContinueToNextLine = false;

    public static DialogueManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene.");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);

        choiceText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choiceText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            choices[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (dialogueIsPlaying && InputManager.GetInstance().GetSubmitPressed())
        {
            // Process the submit action (e.g., advance the dialogue)
            ContinueDialogue();

            // Reset the submit pressed flag
            InputManager.GetInstance().RegisterSubmitPressed();
        }
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {
        if (!dialogueIsPlaying)
        {
            currentStory = new Story(inkJson.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);
            ContinueDialogue();
        }
    }

    public bool IsDialoguePlaying()
    {
        return dialogueIsPlaying;
    }
    
    public void ContinueDialogue()
    {
        if (currentStory != null && dialogueIsPlaying)
        {
            if (currentStory.canContinue)
            {
                dialogueText.text = currentStory.Continue();
                DisplayResponses();
            }
            else
            {
                if (currentStory.currentChoices.Count == 0)
                {
                    StartCoroutine(ExitDialogueMode());
                }
            }
        }
    }

    private void DisplayResponses()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        for (int i = 0; i < choices.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                choices[i].SetActive(true);
                choiceText[i].text = currentChoices[i].text;
            }
            else
            {
                choices[i].SetActive(false);
            }
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0]);
    }

    public void MakeDecision(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        responseSelected = true;
    }

    private void CloseDialogue()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentStory = null;
        responseSelected = false; // Reset response selection flag
    }
    
    private IEnumerator ExitDialogueMode() 
    {
        yield return new WaitForSeconds(0.2f);
        CloseDialogue();
    }
}
