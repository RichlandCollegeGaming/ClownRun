using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;
    public string triggerParameter = "Play"; // Set this in Animator

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            animator.SetTrigger(triggerParameter);
        }
    }
}