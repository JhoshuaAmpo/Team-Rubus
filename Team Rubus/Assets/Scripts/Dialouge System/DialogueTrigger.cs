using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    public enum NPC {None, Syl, Luna, Oran}
    [SerializeField]
    private NPC npc;

    [Header("Visual Cue")]
    [SerializeField]
    private GameObject visualCue;

    [SerializeField]
    private Sprite portrait;

    [SerializeField]
    private UnityEngine.UI.Image blackScreen;

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private PlayerControls playerControls;
    private bool playerInRange;

    private GameObject player;

    private void Awake() {
        playerInRange = false;
        playerControls = new();
        playerControls.Interaction.Enable();
        playerControls.Interaction.Talk.performed += ActivateTalk;
        visualCue.SetActive(false);
    }

    private void Start() {
        if(blackScreen) {
            blackScreen.enabled = false;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name +  " has entered my box!");
        if (other.CompareTag("Player")) {
            playerInRange = true;
            visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log(other.name +  " has exited my box!");
        if (other.CompareTag("Player")) {
            playerInRange = false;
            visualCue.SetActive(false);
        }
    }

    private void ActivateTalk(InputAction.CallbackContext context)
    {
        if(playerInRange && !DialogueManager.Instance.DialougeIsPlaying) {
            visualCue.SetActive(true);
            DialogueManager.Instance.EnterDialogueMode(inkJSON,portrait);
            switch (npc) {
                case NPC.Syl:
                    Debug.Log("Talking to Syl");
                    player.GetComponent<PlayerSideScrollMovement>().MultiplyJumpForce(1.5f);
                    player.GetComponent<PlayerHealth>().DecreaseHealth(30f);
                break;
                case NPC.Luna:
                    Debug.Log("Talking to Luna");
                    player.GetComponent<PlayerHealth>().MultiplyDecayRate(0.5f);
                    player.GetComponent<PlayerHealth>().DecreaseHealth(60f);
                break;
                case NPC.Oran:
                    if (blackScreen.enabled) { break; }
                    StartCoroutine(ProcessEnd());
                break;
                default:
                break;
            }
            gameObject.GetComponent<DialogueTrigger>().enabled = false;
        }
    }

    private IEnumerator ProcessEnd() {
        
        blackScreen.enabled = true;
        float countdown = 60f;
        float fadeRate = 2/60f;
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        while (countdown > 0) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + fadeRate * Time.deltaTime);
            countdown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
