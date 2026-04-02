using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Test : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accel = 25f;
    [SerializeField] float rotationSpeed = 3f;
    public Vector2 MoveInput;

    Rigidbody rb;
    Vector3 moveInput;

    public Vector3 CurrentMoveDirection { get; private set; }

    //Jump Variables
    [SerializeField] float jumpForce = 9.05f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundRadius = 0.25f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float lateTime = 0.12f;
    [SerializeField] float jumpBufferTime = 0.12f;
    float lastGroundedTime;
    float lastJumpPressedTime;
    bool isGrounded;

    //Gravity Jump variables
    [SerializeField] float gravityMultiplier = 29f;

    MovePlatform currentPlatform;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastJumpPressedTime = -999f;
    }

    

    private void FixedUpdate()
    {
        //Move

        Vector3 moveDir = new Vector3(MoveInput.x, 0, MoveInput.y);
        if(moveDir.magnitude > 1f)
        {
            moveDir.Normalize();
        }

        CurrentMoveDirection = moveDir;
        

        Vector3 vel = rb.linearVelocity;

        Vector3 platformVel = Vector3.zero;
        if(currentPlatform != null)
        {
            platformVel = currentPlatform.Delta / Time.fixedDeltaTime;
        }

        Vector3 targetVel = new Vector3(moveDir.x * moveSpeed + platformVel.x, vel.y, moveDir.z * moveSpeed + platformVel.z);
        rb.linearVelocity = Vector3.MoveTowards(vel, targetVel, accel * Time.fixedDeltaTime);

        if(moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(newRot);
            
        }




        //Jump
        isGrounded = CheckGround();
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
        }
        bool jumpBuffered = Time.time - lastJumpPressedTime <= jumpBufferTime;
        bool canJump = isGrounded || (Time.time - lastGroundedTime <= lateTime);

        if (jumpBuffered && canJump)
        {
            lastJumpPressedTime = -999f;

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (!isGrounded)
        {
            if (rb.linearVelocity.y < 0f)
            {
                rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
            }
        }
        
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue button)
    {
        if (button.isPressed)
        {
            lastJumpPressedTime = Time.time;
        }
    }



    bool CheckGround()
    {
        return Physics.CheckSphere(groundCheck.position + Vector3.up * 0.01f, groundRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

    void OnCollisionStay(Collision collision)
    {
        MovePlatform platform = collision.collider.GetComponentInParent<MovePlatform>();
        if (platform == null) return;

        for(int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if(normal.y > 0.5f)
            {
                currentPlatform = platform;
                return;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        MovePlatform platform = collision.collider.GetComponentInParent<MovePlatform>();
        if(platform != null && currentPlatform == platform)
        {
            currentPlatform = null;
        }
    }
}
