using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class yl_PauseManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject settingsPanel;

    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Key")]
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;
    private bool isInSettings = false;

    void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        LockCursor(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (!isPaused)
            {
                OpenPause();
            }
            else if (isInSettings)
            {
                BackToPause();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void OpenPause()
    {
        isPaused = true;
        isInSettings = false;

        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        LockCursor(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        isInSettings = false;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        LockCursor(true);
    }

    public void OpenSettings()
    {
        isInSettings = true;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        isInSettings = false;

        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void LeaveRoom()
    {
        LockCursor(false);

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    public override string ToString()
    {
        return isPaused ? "Paused" : "Playing";
    }

    public void OnLeftRoom()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}