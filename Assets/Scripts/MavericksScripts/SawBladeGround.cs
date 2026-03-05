using UnityEngine;

public class SawBladeGround : MonoBehaviour
{
    [SerializeField] Transform ptA;
    [SerializeField] Transform ptB;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float knockbackForce = 20f;
    

    Transform targetPoint;

    private void Start()
    {
        if(Vector3.Distance(transform.position, ptA.position) < Vector3.Distance(transform.position, ptB.position)) //Which point is furthest from me?
        {
            targetPoint = ptB; //if true ptB is furthest
        }
        else
        {
            targetPoint = ptA; //if false ptA is furthest
        }
    }

    private void Update()
    {
        //move toward the current target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        //if we reached the target, switch direction
        if(Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            if(targetPoint == ptA)
            {
                targetPoint = ptB;
            }
            else
            {
                targetPoint = ptA;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        

        Rigidbody rb = collision.rigidbody;

        if(rb != null)
        {
            Vector3 dir = collision.transform.position - transform.position;
            dir.y = 0.5f;
            dir.Normalize();

            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }
    }

}
