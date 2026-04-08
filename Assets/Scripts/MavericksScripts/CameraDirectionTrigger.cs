using UnityEngine;

public class CameraDirectionTrigger : MonoBehaviour
{
    [SerializeField] CameraBehavior.CameraPathDirection newDirection;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CameraBehavior cam = FindAnyObjectByType<CameraBehavior>();
        if(cam != null)
        {
            cam.SetDirection(newDirection);
        }
    }
}
