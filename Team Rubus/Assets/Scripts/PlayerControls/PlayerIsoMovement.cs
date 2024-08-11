using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerIsoMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    // private AudioSource footStepNoises;
    PlayerControls playerControls;
    CharacterController characterController;
    // Animator animator;


    void Awake() {
        playerControls = new();
        playerControls.Movement.Enable();
        characterController = GetComponent<CharacterController>();
        // animator = transform.root.GetComponentInChildren<Animator>();
        // footStepNoises.Pause();
    }

    private void OnEnable() {
        playerControls.Movement.Enable();
    }

    private void OnDisable() {
        playerControls.Movement.Disable();
    }

    private void Update() {
        Move();
    }

    private void Move(){
        int forwardDir = (int)playerControls.Movement.Forward.ReadValue<float>();
        int sideDir = (int)playerControls.Movement.Strafe.ReadValue<float>();
        // animator.SetBool("IsRun", forwardDir != 0 || sideDir != 0);
        Vector3 moveVelocity = Vector3.zero;
        if (forwardDir != 0 && sideDir != 0) {
            Vector3 moveDir = forwardDir * transform.forward + sideDir * transform.right;
            moveDir.y = 0f;
            moveDir = moveDir.normalized;
            moveVelocity = moveSpeed * moveDir;
        }
        else if (forwardDir != 0) {
            moveVelocity = forwardDir * moveSpeed  * transform.forward;
        }
        else if (sideDir != 0) {
            moveVelocity = sideDir * moveSpeed  * transform.right;
        }
        // footStepNoises.enabled = forwardDir != 0  || sideDir != 0;
        // animator.SetBool("Run", animRun);
        characterController.SimpleMove(moveVelocity);
    }
}
