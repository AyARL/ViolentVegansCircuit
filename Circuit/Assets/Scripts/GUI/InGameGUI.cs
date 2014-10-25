using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class InGameGUI : MenuBase
{
    [SerializeField]
    private GameObject pauseScreen = null;

    [SerializeField]
    private GameObject confirmScreen = null;

    public UnityAction OnPauseGame { get; set; }
    public UnityAction OnResumeGame { get; set; }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        if (OnPauseGame != null)
        {
            OnPauseGame();
        }
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        if(OnResumeGame != null)
        {
            OnResumeGame();
        }
    }

    public void Restart()
    {
        confirmScreen.SetActive(true);
        ConfirmationMenuGUI menu = confirmScreen.GetComponent<ConfirmationMenuGUI>();
        if(menu != null)
        {
            menu.Initialise("Do you want to Restart current level?", RestartLevel, Resume);
            if (OnPauseGame != null)
            {
                OnPauseGame();
            }
        }
        pauseScreen.SetActive(false);
    }

    private void QuitToMenu()
    {
        confirmScreen.SetActive(true);
        ConfirmationMenuGUI menu = confirmScreen.GetComponent<ConfirmationMenuGUI>();
        if (menu != null)
        {
            menu.Initialise("Do you want to quit to main menu?", Quit, Pause);
        }
        pauseScreen.SetActive(false);
    }

    private void Quit()
    {
        Time.timeScale = 1f;
        LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.MainMenuIndex);
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f;
        LoadingManager.LoadLevel(Application.loadedLevel);
    }


}
