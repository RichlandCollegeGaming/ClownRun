using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceGameManager : MonoBehaviour
{
    [SerializeField] GameObject resultsPanel;
    [SerializeField] TMP_Text resultsText;

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
        if (resultsText == null) return;

        if (resultsText != null)
        {
            string results = "";
            if(finishOrder.Count == 0)
            {
                results = "No players finished";
            }
            else
            {
                for (int i = 0; i < finishOrder.Count; i++)
                {
                    int place = i + 1;
                    results += place + ". " + finishOrder[i] + "\n";
                }

                //Show DNF players
                if(deadPlayers.Count > 0)
                {
                    results += "\n";
                    for (int i = 0; i < deadPlayers.Count; i++)
                    {
                        results += "DNF: " + deadPlayers[i] + "\n";
                    }
                }
            }
                

            resultsText.text = results;
        }
    }
}
