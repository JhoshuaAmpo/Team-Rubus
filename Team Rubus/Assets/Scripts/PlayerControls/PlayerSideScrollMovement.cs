using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSideScrollMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    [Tooltip("Determines additional gravity to be applied. 0 = Normal Gravity, 1 = Double Gravity")]
    [Range(0, 10)]
    private float gravMult;

    // [SerializeField]
    // private AudioSource footStepNoises;
    PlayerControls playerControls;

    private Rigidbody rb;
    private Vector3 playerVelocity;
    private bool IsGrounded;
    private Vector3 moveForce;

    // Animator animator;

    void Awake() {
        rb = GetComponent<Rigidbody>();

        playerControls = new();
        playerControls.Movement.Enable();
        playerControls.Movement.Jump.performed += ActivateJump;
        // animator = transform.root.GetComponentInChildren<Animator>();
        // footStepNoises.Pause();
        playerVelocity = Vector3.zero;
    }

    

    private void OnEnable() {
        playerControls.Movement.Enable();
    }

    private void OnDisable() {
        playerControls.Movement.Disable();
    }

    private void Update() {
        SetIsGrounded();
        Move();
        if(!IsGrounded) {
            ApplyMoreGravity();
        }
    }

    private void Move(){
        int sideDir = (int)playerControls.Movement.Strafe.ReadValue<float>();
        moveForce = moveSpeed * sideDir * transform.right;
        rb.AddForce(moveForce,ForceMode.Force);
    }

    private void ActivateJump(InputAction.CallbackContext context) {
        Jump();
    }

    private void Jump() {
        if(!IsGrounded) { return; }
        rb.AddForce(new(0,jumpForce,0), ForceMode.Impulse);
    }

    private void SetIsGrounded() {
        // Debug.Log("Y-Vel: " + rb.velocity + "\nY-Force: " + rb.GetAccumulatedForce().y);
        // IsGrounded = Mathf.Approximately(rb.velocity.y,0) && Mathf.Approximately(rb.GetAccumulatedForce().y,0);
        Debug.Log("Y-Vel: " + rb.velocity);
        IsGrounded = Mathf.Approximately(rb.velocity.y,0);
        
    }

    private void ApplyMoreGravity()
    {
        rb.AddForce(Physics.gravity * gravMult,ForceMode.Force);
    }
}
