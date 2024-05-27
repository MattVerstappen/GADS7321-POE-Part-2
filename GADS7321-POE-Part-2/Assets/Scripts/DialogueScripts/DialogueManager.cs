using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // Typing speed for displaying dialogue text
    [Header("Typing Speed Set up")]
    [SerializeField] private float typingSpeed = 0.04f;

    // JSON file containing global variables for the dialogue
    [Header("Load Globals JSON file")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    // UI elements for displaying dialogue
    [Header("Dialogue UI Management")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    // UI elements for displaying choices
    [Header("Choices UI Management")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private DialogueVariableTracker dialogueVariables;
    private InkExternalFunctions inkExternalFunctions;

    // Called when the DialogueManager object is created
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        // Initialize dialogue variable tracker and Ink external functions
        dialogueVariables = new DialogueVariableTracker(loadGlobalsJSON);
        inkExternalFunctions = new InkExternalFunctions();
    }

    // Retrieves the DialogueManager instance
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    // Called when the DialogueManager object is enabled
    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // Get the layout animator
        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        // Get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            index++;
        }
    }

    // Called every frame
    private void Update()
    {
        // Check if dialogue is not playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        // Check if player can continue to the next line and input is received
        if (canContinueToNextLine && dialogueIsPlaying && InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory(); // Called from Update, allows player to continue through dialogue
            InputManager.GetInstance().RegisterSubmitPressed();
        }
    }

    // Initiates a dialogue sequence
    public void EnterDialogueMode(TextAsset inkJSON, Animator emoteAnimator)
    {
        // Initialize a new Ink story
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // Start listening for dialogue variables
        dialogueVariables.StartListening(currentStory);
        // Bind external functions for Ink script
        inkExternalFunctions.Bind(currentStory, emoteAnimator);

        // Set default display name and portrait animation
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory(); // Start the dialogue
    }

    // Exits the dialogue mode
    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        // Stop listening for dialogue variables
        dialogueVariables.StopListening(currentStory);
        // Unbind external functions
        inkExternalFunctions.Unbind(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    // Continues the dialogue
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    // Displays a line of dialogue with a typewriter effect
    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (InputManager.GetInstance().GetSubmitPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    // Hides choice buttons
    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    // Handles special tags in dialogue lines
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    // Displays choices for the player to select
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    // Automatically selects the first choice
    private IEnumerator SelectFirstChoice()
    {
        // Ensure that no UI element is selected
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        // Select the first choice button
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    // Handles player's choice selection
    public void MakeChoice(int choiceIndex)
    {
        // Check if the player can continue to the next line
        if (canContinueToNextLine) 
        {
            // Choose the selected choice index
            currentStory.ChooseChoiceIndex(choiceIndex);
            // Register submit input press
            InputManager.GetInstance().RegisterSubmitPressed(); // this is specific to my InputManager script
            ContinueStory(); // Proceed to the next part of the story
        }
    }

    // Retrieves the value of a dialogue variable
    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        // Retrieve the value of the specified variable
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }

    // Called when the application quits, saves dialogue variables
    public void OnApplicationQuit()
    {
        dialogueVariables.SaveVariables();
    }
}

