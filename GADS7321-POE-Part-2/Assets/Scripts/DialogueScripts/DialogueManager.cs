using TMPro;
using UnityEngine;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{
    [Header("Typing Speed Setup")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON file")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI Management")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

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
    private const string AUDIO_TAG = "audio";

    private DialogueVariableTracker dialogueVariables;
    
    [Header("Scramble Effect")]
    [SerializeField] private ADHDDisruptionSystem scrambleEffectController;
    
    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    [SerializeField] private bool makePredictable;
    private DialogueAudioInfoSO currentAudioInfo;
    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        dialogueVariables = new DialogueVariableTracker(loadGlobalsJSON);
        
        audioSource = this.gameObject.AddComponent<AudioSource>();
        currentAudioInfo = defaultAudioInfo;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        InitializeAudioInfoDictionary();
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 && InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        SetCurrentAudioInfo(defaultAudioInfo.id);
    }

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
            nextLine = ApplyDisruptionEffects(nextLine);

            // Debug log to print the text that will be displayed
            Debug.Log("Displaying text: " + nextLine);

            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private string ApplyDisruptionEffects(string text)
    {
        var regex = new Regex(@"<disruption>(.*?)<\/disruption>");
        var matches = regex.Matches(text);

        foreach (Match match in matches)
        {
            string disruptedText = match.Groups[1].Value;
            string scrambledText = ADHDDisruptionSystem.ApplyDisruption(disruptedText);
            text = text.Replace(match.Value, scrambledText);
        }

        return text;
    }

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
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine = true;
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

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
                case AUDIO_TAG: 
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

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

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            InputManager.GetInstance().RegisterSubmitPressed();
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
    
    private void InitializeAudioInfoDictionary() 
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach (DialogueAudioInfoSO audioInfo in audioInfos) 
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id) 
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if (audioInfo != null) 
        {
            this.currentAudioInfo = audioInfo;
        }
        else 
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }
    }
    
    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        // set variables for the below based on our config
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;

        // play the sound based on the config
        if (currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource) 
            {
                audioSource.Stop();
            }
            AudioClip soundClip = null;
            // create predictable audio from hashing
            if (makePredictable) 
            {
                int hashCode = currentCharacter.GetHashCode();
                // sound clip
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];
                // pitch
                int minPitchInt = (int) (minPitch * 100);
                int maxPitchInt = (int) (maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                // cannot divide by 0, so if there is no range then skip the selection
                if (pitchRangeInt != 0) 
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
                else 
                {
                    audioSource.pitch = minPitch;
                }
            }
            // otherwise, randomize the audio
            else 
            {
                // sound clip
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                // pitch
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }
            
            // play sound
            audioSource.PlayOneShot(soundClip);
        }
    }

    public void OnApplicationQuit()
    {
        dialogueVariables.SaveVariables();
    }
}
