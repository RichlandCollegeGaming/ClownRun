using UnityEngine;

public class PlayerController_Test : MonoBehaviour
{
    public float moveSpeed = 10f;

    Rigidbody rb;
    Vector3 moveInput;

    //Jump Variables
    [SerializeField] float jumpForce = 10f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundRadius = 0.25f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float lateTime = 0.12f;
    [SerializeField] float jumpBufferTime = 0.12f;
    float lastGroundedTime;
    float lastJumpPressedTime;
    bool isGrounded;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastJumpPressedTime = -999f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastJumpPressedTime = Time.time;
        }


    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); //A and D keys
        float moveZ = Input.GetAxisRaw("Vertical"); //W and S keys

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        Vector3 playerPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(playerPos);

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
        
    }
    bool CheckGround()
    {
        return Physics.CheckSphere(groundCheck.position + Vector3.up * 0.01f, groundRadius, groundMask, QueryTriggerInteraction.Ignore);
    }
}
