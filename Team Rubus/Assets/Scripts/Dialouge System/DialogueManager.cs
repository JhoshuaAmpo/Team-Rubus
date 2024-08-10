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

    private Story currentStory;
    private bool dialougeIsPLaying;
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
        dialougeIsPLaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update() {
        if (!dialougeIsPLaying) { return; }

    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        currentStory = new Story(inkJSON.text);
        dialougeIsPLaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialougeIsPLaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory() {
        if (currentStory.canContinue) {
            dialogueText.text = currentStory.Continue();
        }
        else {
            ExitDialogueMode();
        }
    }

    private void ActivateContinueStory(InputAction.CallbackContext context)
    {
        ContinueStory();
    }
}