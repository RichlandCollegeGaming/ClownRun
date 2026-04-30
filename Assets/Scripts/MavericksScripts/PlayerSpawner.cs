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
        Transform spawnPoint = SpawnPoints[index];

        if (spawnPoint == null) return;

        //Select Visual
        Transform chosenVisual = SetPlayerVisual(playerInput.transform, m_playerCount);

        //Assign Animator
        Animator anim = null;
        if(chosenVisual != null)
        {
            anim = chosenVisual.GetComponentInChildren<Animator>();
        }

        PlayerController_Test controller = playerInput.GetComponent<PlayerController_Test>();
        if(controller != null && anim != null)
        {
            controller.SetAnimator(anim);
        }

        TeleportWholePlayer(playerInput.transform, spawnPoint.position, spawnPoint.rotation);

        if(cam != null)
        {
            //Register player with Camera
            cam.RegisterPlayer(playerInput.transform);
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

    Transform SetPlayerVisual(Transform playerRoot, int playerIndex)
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

        return chosen;
    }
}
