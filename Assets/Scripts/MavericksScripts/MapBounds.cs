using UnityEngine;
using UnityEngine.InputSystem;

public class MapBounds : MonoBehaviour
{
    CameraBehavior cam;


    private void Start()
    {
        cam = FindAnyObjectByType<CameraBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponent<PlayerInput>();
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
