using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject mapSelectPanel;

    //Select first button for each window
    [SerializeField] Button mainFirstButton;
    [SerializeField] Button settingsFirstButton;
    [SerializeField] Button controlsFirstButton;
    [SerializeField] Button mapSelectFirstButton;

    //Select Map Buttons
    [SerializeField] Button firstMap;
    [SerializeField] Button secondMap;
    [SerializeField] Button thirdMap;
    [SerializeField] Button fourthMap;

    //Play
    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        mapSelectPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(mapSelectFirstButton.gameObject);
    }

    //Choose Map and Load the Scene
    public void PlaySelected()
    {
        int index = -1;

        if (firstMap.interactable) index = 1;
        else if (secondMap.interactable) index = 2;
        else if (thirdMap.interactable) index = 3;
        else if (fourthMap.interactable) index = 4;

        if(index == -1)
        {
            Debug.LogError("No map selected!");
            return;
        }

        LoadSceneSafe(index);
    }

    public void CloseMapSelection()
    {
        mapSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(mainFirstButton.gameObject);
    }

    //Safe Helper
    void LoadSceneSafe(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogError("Scene index out of range: " + index);
        }
    }

    //Settings
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(settingsFirstButton.gameObject);
    }

    public void closeSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(mainFirstButton.gameObject);
    }

    //Controls
    public void OpenControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(controlsFirstButton.gameObject);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(mainFirstButton.gameObject);
    }

    //Exit
    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
