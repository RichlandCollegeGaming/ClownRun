using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] float knockbackForce = 20f;
    [SerializeField] Vector3 knockbackDirection = new Vector3(0, 0, 1);

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;

        if (rb != null)
        {
            Vector3 dir = transform.TransformDirection(knockbackDirection).normalized;
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }
    }
}