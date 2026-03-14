using UnityEngine;
using UnityEngine.InputSystem;

public class DeathBounds : MonoBehaviour
{
    CameraBehavior cam;

    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInput player = other.GetComponentInParent<PlayerInput>();

        if(player != null)
        {
            if(cam != null)
            {
                cam.RemovePlayer(player.transform);
            }
           Destroy(player.gameObject);
        }
        
    }
}
