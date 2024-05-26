using System;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Ink.UnityIntegration;

public class DialogueManager : MonoBehaviour
{
    [Header("Tweaks")] 
    [SerializeField] private float displaySpeed = 0.02f;

    [Header("Global Variable Loader")] 
    [SerializeField] private TextAsset loadGlobalsFromJSON;
    
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimation;
    [SerializeField] private GameObject continueIndicator;
    private Animator layoutSwitch;
    
    [Header("Response UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choiceText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; } = false;
    private bool responseSelected = false;
    private bool canContinueToNextLine = false;

    public static DialogueManager instance { get; private set; }
    private Coroutine typeLineOutCoroutine;
    private const string SPEAKERTAG = "speaker";
    private const string SPEAKERPORTRAIT = "portrait";
    private const string SPEAKERLAYOUT = "layout";

    private DialogueVariableTracker dialogueVariableTracker;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene.");
        }
        instance = this;
        dialogueVariableTracker = new DialogueVariableTracker(loadGlobalsFromJSON);
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        dialoguePanel.SetActive(false);

        layoutSwitch = dialoguePanel.GetComponent<Animator>();
        
        choiceText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choiceText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
            choices[i].SetActive(false);
        }
    }
    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        
        if (currentStory.currentChoices.Count == 0 && InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueDialogue();
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
            dialogueVariableTracker.ListenAtStart(currentStory);

            displayNameText.text = "???";
            portraitAnimation.Play("DefaultAnimation");
            layoutSwitch.Play("SpeakerRight");
            
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
                if (typeLineOutCoroutine != null)
                {
                    StopCoroutine(typeLineOutCoroutine);
                }
                typeLineOutCoroutine = StartCoroutine(TypeDisplayLine(currentStory.Continue()));
                //dialogueText.text = currentStory.Continue();

                HandleTags(currentStory.currentTags);
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
    private IEnumerator TypeDisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        continueIndicator.SetActive(false);
        HideChoicesAtStart();
        canContinueToNextLine = false;
        bool hasStyleApplied = false;
        foreach (char letter in line.ToCharArray())
        {
            if (InputManager.GetInstance().GetSubmitPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            if (letter == '<' || hasStyleApplied)
            {
                hasStyleApplied = true;
                if (letter == '>')
                {
                    hasStyleApplied = false;
                }
            }
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(displaySpeed);
            }
        }
        continueIndicator.SetActive(false);
        DisplayResponses();
        canContinueToNextLine = true;
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.Log("WUPSIES could not sort this out mate... No Bueno..." + tag);
            }

            string keyTag = splitTag[0].Trim();
            string valueOfTag = splitTag[1].Trim();

            switch (keyTag)
            {
                case SPEAKERTAG:
                    displayNameText.text = valueOfTag;
                    Debug.Log("Speaker is = " + valueOfTag);
                    break;
                case SPEAKERPORTRAIT:
                    portraitAnimation.Play(valueOfTag);
                    Debug.Log("Speaker portrait is = " + valueOfTag);
                    break;
                case SPEAKERLAYOUT:
                    layoutSwitch.Play(valueOfTag);
                    Debug.Log("Speaker layout is = " + valueOfTag);
                    break;
                default:
                    Debug.Log("You didn't set up the narrative script properly you moron." + tag);
                    break;
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
    private void HideChoicesAtStart()
    {
        foreach (GameObject currentChoices in choices)
        {
            currentChoices.SetActive(false);
        }
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
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            InputManager.GetInstance().RegisterSubmitPressed();
            ContinueDialogue();
            //responseSelected = true;
        }
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
        dialogueVariableTracker.ListeningEnd(currentStory);
        CloseDialogue();
    }

    public void OnApplicationQuit()
    {
        if (dialogueVariableTracker != null)
        {
            dialogueVariableTracker.SaveVariables();
        }
    }
}
