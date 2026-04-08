using UnityEngine;

public class FlyingAxe : MonoBehaviour
{
    [SerializeField] Transform ptA;
    [SerializeField] Transform ptB;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float spinSpeed = -400f;
    [SerializeField] float knockbackForce = 20f;


    private void Start()
    {
        transform.position = ptA.position;
    }

    private void Update()
    {
        //move toward the current target
        transform.position = Vector3.MoveTowards(transform.position, ptB.position, moveSpeed * Time.deltaTime);

        //if we reached the target, switch direction
        if (Vector3.Distance(transform.position, ptB.position) < 0.05f)
        {
            transform.position = ptA.position;
        }

        transform.Rotate(spinSpeed * Time.deltaTime, 0f, 0f);

    }

    private void OnCollisionEnter(Collision collision)
    {


        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            Vector3 dir = collision.transform.position - transform.position;
            dir.y = 0.5f;
            dir.Normalize();

            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }
    }
}
