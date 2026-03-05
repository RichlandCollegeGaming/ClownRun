using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CameraBehavior : MonoBehaviour
{
    List<Transform> players = new List<Transform>();


    [SerializeField] float smoothTime = 0.15f;
    [SerializeField] float z = -39.5f;

    [SerializeField] float forwardOffset = 3f;
    [SerializeField] float heightOffset = 8f;
    [SerializeField] float deadZone = 1.0f;

    float camX;
    float camY;

    float velX;
    float velY;

    public void RegisterPlayer(Transform player)
    {
        players.Add(player);
    }

    private void Start()
    {
        camX = transform.position.x;
        camY = transform.position.y;
    }

    private void LateUpdate()
    {
        if(players.Count == 0) return;

        //Find leader
        Transform leader = players[0];
        for(int i = 1; i < players.Count; i++)
        {
            if (players[i] != null && players[i].position.x > leader.position.x)
            {
                leader = players[i];
            }
        }

        float desiredX = leader.position.x + forwardOffset;
        float desiredY = leader.position.y + heightOffset;

        //Deadzone + never move backward
        if(desiredX > camX + deadZone)
        {
            camX = Mathf.SmoothDamp(camX, desiredX, ref velX, smoothTime);
            camY = Mathf.SmoothDamp(camY, desiredY, ref velY, smoothTime);
        }

        transform.position = new Vector3(camX, camY, z);
    }







    private void OnDrawGizmos()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float distance = Mathf.Abs(cam.transform.position.y);

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0, distance));
        Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, distance));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, distance));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
