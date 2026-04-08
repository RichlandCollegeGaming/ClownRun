using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CameraBehavior : MonoBehaviour
{
    public enum CameraPathDirection
    {
        Right,
        Left,
        Up,
        Down
    }


    List<Transform> players = new List<Transform>();


    [SerializeField] float smoothTime = 0.15f;
    [SerializeField] float z = -39.5f;

    [SerializeField] float forwardOffset = 3f;
    [SerializeField] float heightOffset = 8f;
    [SerializeField] float deadZone = 1.0f;

    [SerializeField] CameraPathDirection currentDirection = CameraPathDirection.Right;

    float camX;
    float camY;

    float velX;
    float velY;

    public void RegisterPlayer(Transform player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }

    public void RemovePlayer(Transform player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
    }

    public void SetDirection(CameraPathDirection newDirection)
    {
        currentDirection = newDirection;
    }

    private void Start()
    {
        camX = transform.position.x;
        camY = transform.position.y;
    }

    private void LateUpdate()
    {
        players.RemoveAll(p => p == null);

        if(players.Count == 0) return;

        //Find leader
        Transform leader = players[0];
        for(int i = 1; i < players.Count; i++)
        {
            if (IsAhead(players[i], leader))
            {
                leader = players[i];
            }
        }

        float desiredX = camX;
        float desiredY = camY;

        switch (currentDirection)
        {
            case CameraPathDirection.Right:
                desiredX = leader.position.x + forwardOffset;
                desiredY = leader.position.y + heightOffset;
                break;

            case CameraPathDirection.Left:
                desiredX = leader.position.x - forwardOffset;
                desiredY = leader.position.y + heightOffset;
                break;

            case CameraPathDirection.Up:
                desiredX = leader.position.x;
                desiredY = leader.position.y + heightOffset;
                break;

            case CameraPathDirection.Down:
                desiredX = leader.position.x;
                desiredY = leader.position.y - heightOffset;
                break;
        }

        //Deadzone + never move backward
        if(Mathf.Abs(desiredX - camX) > deadZone)
        {
            camX = Mathf.SmoothDamp(camX, desiredX, ref velX, smoothTime);
        }
        if(Mathf.Abs(desiredY - camY) > deadZone)
        {
            camY = Mathf.SmoothDamp(camY, desiredY, ref velY, smoothTime);
        }

        transform.position = new Vector3(camX, camY, z);
    }

    bool IsAhead(Transform challenger, Transform currentLeader)
    {
        switch (currentDirection)
        {
            case CameraPathDirection.Right:
                return challenger.position.x > currentLeader.position.x;

            case CameraPathDirection.Left:
                return challenger.position.x < currentLeader.position.x;

            case CameraPathDirection.Up:
                return challenger.position.y > currentLeader.position.y;

            case CameraPathDirection.Down:
                return challenger.position.y < currentLeader.position.y;
        }

        return false;
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
