using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = m_playerCount % SpawnPoints.Length;
        Vector3 spawnPos = SpawnPoints[index].position;

        Rigidbody rb = playerInput.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = spawnPos;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            playerInput.transform.position = spawnPos;
        }
            m_playerCount++;
    }
}
