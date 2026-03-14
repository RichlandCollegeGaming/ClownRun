using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;


    CameraBehavior cam;

    HashSet<InputDevice> usedDevices = new HashSet<InputDevice>();

    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
    }



    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput == null) return;
        if(SpawnPoints == null || SpawnPoints.Length == 0) return;

        InputDevice device = null;

        if(playerInput.devices.Count > 0)
        {
            device = playerInput.devices[0];
        }

        if (device == null) return;

        //if this device already had a player before, do not allow rejoin
        if (usedDevices.Contains(device))
        {
            Destroy(playerInput.gameObject);
            return;
        }

        usedDevices.Add(device);


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

        SetPlayerVisual(playerInput.transform, m_playerCount);

        //Register player with Camera
        cam.RegisterPlayer(playerInput.transform);

            m_playerCount++;
    }

    void SetPlayerVisual(Transform playerRoot, int playerIndex)
    {
        string[] names = { "Player1", "Player2", "Player3", "Player4" };

        for(int i = 0; i < playerRoot.childCount; i++)
        {
            playerRoot.GetChild(i).gameObject.SetActive(false);
        }

        int visualIndex = playerIndex % names.Length;
        Transform chosen = playerRoot.Find(names[visualIndex]);

        if(chosen != null)
        {
            chosen.gameObject.SetActive(true);
        }
    }
}
