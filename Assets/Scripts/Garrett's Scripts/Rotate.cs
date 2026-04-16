using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeedX = 50f;
    public float rotateSpeedY = 50f;
    public float rotateSpeedZ = 50f;

    [SerializeField] private bool _isUnscaled = false;

    void Update()
    {
        float rotateX;
        float rotateY;
        float rotateZ;

        // Rotate the object continuously
        if (_isUnscaled)
        {
            rotateX = rotateSpeedX * Time.unscaledDeltaTime;
            rotateY = rotateSpeedY * Time.unscaledDeltaTime;
            rotateZ = rotateSpeedZ * Time.unscaledDeltaTime;
        }
        else
        {
            rotateX = rotateSpeedX * Time.deltaTime;
            rotateY = rotateSpeedY * Time.deltaTime;
            rotateZ = rotateSpeedZ * Time.deltaTime;
        }

        transform.Rotate(Vector3.right, rotateX);
        transform.Rotate(Vector3.up, rotateY);
        transform.Rotate(Vector3.forward, rotateZ);
    }
}
