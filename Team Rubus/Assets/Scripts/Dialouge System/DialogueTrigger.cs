using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField]
    private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake() {
        playerInRange = false;
        // visualCue.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name +  " has entered my box!");
        if (other.CompareTag("Player")) {
            playerInRange = true;
            // visualCue.SetActive(true);
            DialogueManager.Instance.EnterDialogueMode(inkJSON);
        }
    }

    private void OnTriggerExit(Collider other) {
        Debug.Log(other.name +  " has exited my box!");
        if (other.CompareTag("Player")) {
            playerInRange = false;
            // visualCue.SetActive(false);
        }
    }

    
}
