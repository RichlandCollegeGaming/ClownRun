using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LobbyManager : MonoBehaviour
{
    public static bool GameStarted { get; private set; }

    [SerializeField] GameObject joinPanel;
    [SerializeField] Image[] playerIndicators;

    PlayerSpawner playerSpawner;
    PlayerInputManager inputManager;

    [SerializeField] int minimumPlayersToStart = 1;
    int currentPlayers = 0;

    private void Awake()
    {
        playerSpawner = FindAnyObjectByType<PlayerSpawner>();
        inputManager = FindAnyObjectByType<PlayerInputManager>();

        if(playerSpawner == null)
        {
            Debug.LogError("Player Spawner not found");
        }
        if(inputManager == null)
        {
            Debug.LogError("Input manager not found");
        }
    }


    void Start()
    {
        GameStarted = false;

        if(joinPanel != null)
        {
            joinPanel.SetActive(true);
        }
    }

    
    void Update()
    {
        if (GameStarted) return;
        if (playerSpawner == null) return;

        if (playerSpawner.PlayerCount < minimumPlayersToStart) return;

        bool startPressed = false;

        if(Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            startPressed = true;
        }

        foreach(Gamepad pad in Gamepad.all)
        {
            if(pad != null && pad.startButton.wasPressedThisFrame)
            {
                startPressed = true;
                break;
            }
        }

        if (startPressed)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if(GameStarted) return;

        GameStarted = true;

        if(joinPanel != null)
        {
            joinPanel.SetActive(false);
        }

        if(inputManager != null)
        {
            inputManager.DisableJoining();
        }

        Debug.Log("GameStarted");
    }
    public void OnPlayerJoined()
    {
        if(currentPlayers < playerIndicators.Length)
        {
            playerIndicators[currentPlayers].color = Color.green;
            currentPlayers++;
        }
    }
}
