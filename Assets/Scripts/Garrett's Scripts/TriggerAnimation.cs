using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator; // Assign in Inspector
    public string boolParameterName = "IsPlayerInside";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetBool(boolParameterName, true);
            }
            else
            {
                Debug.LogWarning("Animator not assigned!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetBool(boolParameterName, false);
            }
        }
    }
}