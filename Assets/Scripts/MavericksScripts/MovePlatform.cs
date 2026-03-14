using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] Transform ptA;
    [SerializeField] Transform ptB;
    [SerializeField] float moveSpeed = 5f;

    public Vector3 Delta {  get; private set; }

    private float timeToTarget;
    private float elapsedTime;

    Vector3 targetPoint;
    Vector3 previousPoint;

    Rigidbody platformRb;

    private void Start()
    {
        platformRb = GetComponent<Rigidbody>();

        if (Vector3.Distance(platformRb.position, ptA.position) < Vector3.Distance(platformRb.position, ptB.position)) //Which point is furthest from me?
        {
            targetPoint = ptB.position; //if true ptB is furthest
            previousPoint = ptA.position;
        }
        else
        {
            targetPoint = ptA.position; //if false ptA is furthest
            previousPoint = ptB.position;
        }

        float distanceToTarget = Vector3.Distance(previousPoint, targetPoint);
        timeToTarget = distanceToTarget / moveSpeed;
        elapsedTime = 0f;
    }

    private void FixedUpdate()
    {
        Vector3 oldPos = platformRb.position;

        elapsedTime += Time.fixedDeltaTime;

        float elapsedPercentage = elapsedTime / timeToTarget;
        elapsedPercentage = Mathf.Clamp01(elapsedPercentage);
        //elapsedPercentage = Mathf.SmoothStep(0f,1f, elapsedPercentage);

        //move toward the current target
        Vector3 newPos = Vector3.Lerp(previousPoint, targetPoint, elapsedPercentage);
        platformRb.MovePosition(newPos);

        Delta = newPos - oldPos;

        //if we reached the target, switch direction
        if (elapsedPercentage >= 1f)
        {
            TargetNextPoint();
        }
        

    }

    private void TargetNextPoint()
    {
        Vector3 temp = previousPoint;
        previousPoint = targetPoint;
        targetPoint = temp;

        elapsedTime = 0f;

        float distanceToTarget = Vector3.Distance(previousPoint, targetPoint);
        timeToTarget = distanceToTarget / moveSpeed;
    }
}
