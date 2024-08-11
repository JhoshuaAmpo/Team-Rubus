using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices;

    public bool DialougeIsPLaying {get; private set;}

    private TextMeshProUGUI[] choicesText;
    private Story currentStory;


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
        DialougeIsPLaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        DialougeIsPLaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        DialougeIsPLaying = false;
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
            DisplayChoices();
        }
        else {
            ExitDialogueMode();
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
    }
}