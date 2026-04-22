using UnityEngine;

public class PlayerColliderRelay : MonoBehaviour
{
    PlayerController_Test playerHitSound;
    void Start()
    {
        playerHitSound = GetComponentInParent<PlayerController_Test>();
    }

    void OnCollisionEnter(Collision collision)
    {
        bool isObstacle = collision.gameObject.CompareTag("Obstacle");

        if (!isObstacle) return;

        if(playerHitSound != null)
        {
            playerHitSound.PlayHitSound();
        }
    }
    
}
