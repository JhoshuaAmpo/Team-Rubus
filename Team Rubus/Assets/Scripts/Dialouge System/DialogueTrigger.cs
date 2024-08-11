using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    public enum NPC {Syl, Luna, Oran}
    [SerializeField]
    private NPC npc;

    [Header("Visual Cue")]
    [SerializeField]
    private GameObject visualCue;

    [SerializeField]
    private Sprite portrait;

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
                break;
                default:
                break;
            }
        }
    }
}
