using UnityEngine;
using UnityEngine.InputSystem;

public class GoalTrigger : MonoBehaviour
{
    RaceGameManager raceManager;




    void Start()
    {
        raceManager = FindAnyObjectByType<RaceGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInput player = other.GetComponentInParent<PlayerInput>();
        if (player == null) return;

        GameObject playerObj = player.gameObject;

        if(raceManager != null)
        {
            raceManager.PlayerFinished(playerObj);
        }

        //delete player after finishing
        Destroy(playerObj);
    }

}
