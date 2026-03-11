using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    private int m_playerCount;


    CameraBehavior cam;

    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
    }



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
