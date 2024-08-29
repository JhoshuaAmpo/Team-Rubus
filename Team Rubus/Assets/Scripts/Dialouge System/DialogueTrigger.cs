using System.Collections;
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
    private EndGameFadeToBlack endGameFadeToBlack;
    

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;

    private PlayerControls playerControls;
    private bool playerInRange;

    private GameObject player;

    private void Awake() {
        playerInRange = false;
        playerControls = new();
        playerControls.Interaction.Talk.performed += ActivateTalk;
        visualCue.SetActive(false);
    }

    private void Start() {
        
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            visualCue.SetActive(true);
            playerControls.Interaction.Enable();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            visualCue.SetActive(false);
            playerControls.Interaction.Disable();
        }
    }

    private void ActivateTalk(InputAction.CallbackContext context)
    {
        if(playerInRange && !DialogueManager.Instance.DialougeIsPlaying) {
            visualCue.SetActive(true);
            DialogueManager.Instance.EnterDialogueMode(inkJSON,portrait, this.gameObject);
            switch (npc) {
                case NPC.Syl:
                    Debug.Log("Talking to Syl");
                    player.GetComponent<PlayerSideScrollMovement>().MultiplyJumpForce(1.5f);
                    player.GetComponent<PlayerHealth>().DecreaseHealth(30f);
                    gameObject.GetComponent<DialogueTrigger>().enabled = false;
                break;
                case NPC.Luna:
                    Debug.Log("Talking to Luna");
                    player.GetComponent<PlayerHealth>().MultiplyDecayRate(0.5f);
                    player.GetComponent<PlayerHealth>().DecreaseHealth(60f);
                    gameObject.GetComponent<DialogueTrigger>().enabled = false;
                break;
                case NPC.Oran:
                    endGameFadeToBlack.ProcessEnd();
                break;
                default:
                break;
            }
            playerControls.Interaction.Disable();
        }
    }

    
}
