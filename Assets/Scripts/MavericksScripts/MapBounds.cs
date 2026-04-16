using UnityEngine;
using UnityEngine.InputSystem;

public class MapBounds : MonoBehaviour
{
    CameraBehavior cam;
    RaceGameManager raceManager;


    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
        raceManager = FindAnyObjectByType<RaceGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
        if (player == null) return;

        GameObject playerObj = player.gameObject;

        if (player != null)
        {
            if(cam != null)
            {
                cam.RemovePlayer(player.transform);
            }
            if(raceManager != null)
            {
                raceManager.PlayerDied(playerObj);
            }
            
            Destroy(playerObj);
        }
    }


}
