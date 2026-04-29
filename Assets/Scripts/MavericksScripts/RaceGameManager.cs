using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceGameManager : MonoBehaviour
{
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TMP_Text[] resultsText;

    int totalPlayers;
    int alivePlayers;

    public bool RaceEnded { get; private set; }

    List<string> finishOrder = new List<string>();
    HashSet<GameObject> finishedPlayers = new HashSet<GameObject>();
    List<string> deadPlayers = new List<string>();

    public void RestartRace()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }


    private void Start()
    {
        if(resultsPanel != null)
        {
            resultsPanel.SetActive(false);
        }
    }

    public void RegisterPlayer(GameObject player)
    {
        totalPlayers++;
        alivePlayers++;
        
    }

    public void PlayerDied(GameObject player)
    {
        if (player == null) return;
        if (finishedPlayers.Contains(player)) return;

        string playerName = player.name;

        if (deadPlayers.Contains(playerName)) return;

        deadPlayers.Add(playerName);
        alivePlayers--;

        CheckForRaceEnd();
    }

    public void PlayerFinished(GameObject player)
    {
        if (player == null) return;
        if (finishedPlayers.Contains(player)) return;

        finishedPlayers.Add(player);
        alivePlayers--;

        finishOrder.Add(player.name);
        CheckForRaceEnd();
    }

    void CheckForRaceEnd()
    {
        
        if(alivePlayers <= 0)
        {
            ShowResults();
        }
    }

    void ShowResults()
    {
        RaceEnded = true;

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
        }

        //Clear all slots first
        for (int i = 0; i < resultsText.Length; i++)
        {
            resultsText[i].text = "DNF";
        }


        //Finished players first
        for (int i = 0; i < finishOrder.Count; i++)
        {
            int playerIndex = GetPlayerIndexFromName(finishOrder[i]);

            if(playerIndex >= 0 && playerIndex < resultsText.Length)
            {
                resultsText[playerIndex].text = GetPlaceText(i + 1);
            }
        }
        
    }
    string GetPlaceText(int place)
    {
        if (place == 1) return "1st Place";
        if (place == 2) return "2nd Place";
        if (place == 3) return "3rd Place";
        if (place == 4) return "4th Place";

        return place + "th Place";
    }
    int GetPlayerIndexFromName(string playerName)
    {
        if (playerName == "Player1") return 0;
        if (playerName == "Player2") return 1;
        if (playerName == "Player3") return 2;
        if (playerName == "Player4") return 3;

        return -1;
    }
}
