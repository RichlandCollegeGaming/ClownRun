using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;

    public int PlayerCount => m_playerCount;

    CameraBehavior cam;
    LobbyManager lobbyManager;
    RaceGameManager raceManager;

    HashSet<InputDevice> usedDevices = new HashSet<InputDevice>();

    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
        raceManager = FindAnyObjectByType<RaceGameManager>();
        lobbyManager = FindAnyObjectByType<LobbyManager>();
    }



    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if(raceManager != null && raceManager.RaceEnded)
        {
            Destroy(playerInput.gameObject);
            return;
        }
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
        Transform spawnPoint = SpawnPoints[index];

        if (spawnPoint == null) return;


        SetPlayerVisual(playerInput.transform, m_playerCount);
        TeleportWholePlayer(playerInput.transform, spawnPoint.position, spawnPoint.rotation);

        //set clean player name
        playerInput.gameObject.name = "Player" + (m_playerCount + 1);

        if(cam != null)
        {
            //Register player with Camera
            cam.RegisterPlayer(playerInput.transform);
        }
        if(raceManager != null)
        {
            raceManager.RegisterPlayer(playerInput.gameObject);
        }
        if(lobbyManager != null)
        {
            lobbyManager.OnPlayerJoined();
        }


            m_playerCount++;
    }

    void TeleportWholePlayer(Transform playerRoot, Vector3 newRootPos, Quaternion newRootRot)
    {

        Vector3 oldRootPos = playerRoot.position;
        Quaternion oldRootRot = playerRoot.rotation;

        Quaternion deltaRot = newRootRot * Quaternion.Inverse(oldRootRot);
        Rigidbody[] bodies = playerRoot.GetComponentsInChildren<Rigidbody>(true);

        for (int i = 0; i < bodies.Length; i++)
        {
            Rigidbody body = bodies[i];
            Vector3 worldOffset = body.position - oldRootPos;
            Vector3 rotatedOffset = deltaRot * worldOffset;

            body.position = newRootPos + rotatedOffset;
            body.rotation = deltaRot * body.rotation;

            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

        playerRoot.SetPositionAndRotation(newRootPos, newRootRot);


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
