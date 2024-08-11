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
    [Range(0, 10)]
    [Tooltip("Determines additional gravity to be applied. 0 = Normal Gravity, 1 = Double Gravity")]
    private float additionalGravityMultiplier;

    [SerializeField]
    float m_MaxDistance;
    [SerializeField]
    Vector3 groundDetectBoxMultiplier;

    // [SerializeField]
    // private AudioSource footStepNoises;
    PlayerControls playerControls;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Vector3 playerVelocity;
    private bool IsGrounded;
    private Vector3 moveForce;
    private Vector3 groundDetectingBox;

    bool m_HitDetect;
    RaycastHit HitInfo;
    

    // Animator animator;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundDetectingBox = new(capsuleCollider.radius * groundDetectBoxMultiplier.x, capsuleCollider.height * groundDetectBoxMultiplier.y, capsuleCollider.radius * groundDetectBoxMultiplier.z);

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
        if (DialogueManager.Instance.DialougeIsPLaying) { return; }
        int sideDir = (int)playerControls.Movement.Strafe.ReadValue<float>();
        moveForce = moveSpeed * sideDir * transform.right;
        rb.AddForce(moveForce,ForceMode.Force);
    }

    private void ActivateJump(InputAction.CallbackContext context) {
        if (DialogueManager.Instance.DialougeIsPLaying) { return; }
        Jump();
    }

    private void Jump() {
        if(!IsGrounded) { return; }
        rb.AddForce(new(0,jumpForce,0), ForceMode.Impulse);
        // Move();
    }

    private void SetIsGrounded() {
        // IsGrounded = Mathf.Approximately(rb.velocity.y,0);
        // m_HitDetect = Physics.BoxCast(transform.position, groundDetectBoxMultiplier, Vector3.down, out HitInfo, Quaternion.identity, collider);
        m_HitDetect = Physics.BoxCast(transform.position, groundDetectingBox, Vector3.down, out HitInfo, Quaternion.identity, capsuleCollider.height);
        if(m_HitDetect) {
            // Debug.Log(HitInfo.collider.tag);
            IsGrounded = HitInfo.collider.CompareTag("Platform") && rb.velocity.y < 0.1f && rb.velocity.y > -0.1f;
        } else {
            IsGrounded = false;
        }
        
    }

    private void ApplyMoreGravity()
    {
        rb.AddForce(Physics.gravity * additionalGravityMultiplier,ForceMode.Force);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, Vector3.down * HitInfo.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + Vector3.down * HitInfo.distance, groundDetectingBox);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            if(!capsuleCollider) { return; }
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector3.down * capsuleCollider.height);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + Vector3.down * capsuleCollider.height, groundDetectingBox);
        }
    }
}
