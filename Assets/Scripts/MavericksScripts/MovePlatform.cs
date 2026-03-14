using UnityEngine;
using System.Collections.Generic;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] Transform ptA;
    [SerializeField] Transform ptB;
    [SerializeField] float moveSpeed = 5f;

    Rigidbody rb;
    Transform targetPoint;

    private void Start()
    {
        if (Vector3.Distance(transform.position, ptA.position) < Vector3.Distance(transform.position, ptB.position)) //Which point is furthest from me?
        {
            targetPoint = ptB; //if true ptB is furthest
        }
        else
        {
            targetPoint = ptA; //if false ptA is furthest
        }
    }

    private void FixedUpdate()
    {
        Vector3 oldPos = transform.position;

        //move toward the current target
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.fixedDeltaTime);

        //if we reached the target, switch direction
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            if (targetPoint == ptA)
            {
                targetPoint = ptB;
            }
            else
            {
                targetPoint = ptA;
            }
        }

        //Move player with platform
        Vector3 delta = transform.position - oldPos;
        if(rb != null)
        {
            rb.MovePosition(rb.position + delta);
        }

    }

    //collision to check if player is on platform
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        for(int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            //Player is on top of platform
            if(normal.y < -0.5f)
            {
                rb = collision.rigidbody;
                return;
            }
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.rigidbody == rb)
        {
            rb = null;
        }
    }
}
