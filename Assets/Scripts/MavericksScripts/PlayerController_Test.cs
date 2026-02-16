using UnityEngine;

public class PlayerController_Test : MonoBehaviour
{
    public float moveSpeed = 10f;

    Rigidbody rb;
    Vector3 moveInput;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); //A and D keys
        float moveZ = Input.GetAxis("Vertical"); //W and S keys

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(playerPos);
    }
}
