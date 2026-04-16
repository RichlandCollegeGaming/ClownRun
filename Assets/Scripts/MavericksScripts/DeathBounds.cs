using UnityEngine;
using UnityEngine.InputSystem;

public class DeathBounds : MonoBehaviour
{
    CameraBehavior cam;
    RaceGameManager raceManager;

    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
        raceManager = FindAnyObjectByType<RaceGameManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInput player = other.GetComponentInParent<PlayerInput>();

        if(player == null) return;
        GameObject playerObj = player.gameObject;

        if (player != null)
        {

            if (cam != null)
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
