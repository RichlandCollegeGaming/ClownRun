using UnityEngine;

public class LockCanvasRotation : MonoBehaviour
{
    private Quaternion fixedRotation;

    private void Awake()
    {
        fixedRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = fixedRotation;
    }
}
