using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject[] choices;

    [SerializeField] private GameObject LeftPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;

    public bool DialougeIsPlaying {get; private set;}

    private TextMeshProUGUI[] choicesText;
    private Story currentStory;
    private Sprite portraitImg;


    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private PlayerControls playerControls;

    private void Awake() {
        if (Instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
            Destroy(this);
        }
        Instance = this;
        playerControls = new();
        playerControls.Interaction.Enable();
        playerControls.Interaction.Talk.performed += ActivateContinueStory;
    }

    private void OnEnable() {
        playerControls.Interaction.Enable();
    }

    private void OnDisable() {
        playerControls.Interaction.Disable();
    }

    private void Start() {
        DialougeIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update() {
        if(!DialougeIsPlaying) { return;}


    }

    public void EnterDialogueMode(TextAsset inkJSON, Sprite portrait) {
        currentStory = new Story(inkJSON.text);
        portraitImg = portrait;
        DialougeIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        DialougeIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ActivateContinueStory(InputAction.CallbackContext context)
    {
        ContinueStory();
    }

    private void ContinueStory() {
        if (currentStory.canContinue) {
            dialogueText.text = currentStory.Continue();
            HandleTages(currentStory.currentTags);
            DisplayChoices();
        }
        else {
            ExitDialogueMode();
        }
    }

    private void HandleTages(List<string> currentTags)
    {
        foreach (string tag in currentTags) 
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) 
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            
            // handle the tag
            switch (tagKey) 
            {
                case SPEAKER_TAG:
                    speakerName.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    LeftPortrait.GetComponent<UnityEngine.UI.Image>().sprite = portraitImg;
                    break;
                case LAYOUT_TAG:
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length) {
            Debug.Log("More choices give then UI can support!");
        }

        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++) {
            choices[i].SetActive(false);
        }

        // StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice() {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}