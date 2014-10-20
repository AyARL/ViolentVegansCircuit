using UnityEngine;
using System.Collections;

public class InGameGUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseScreen = null;

    [SerializeField]
    private GameObject confirmScreen = null;

    public void Pause()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 0;
        confirmScreen.SetActive(true);
        ConfirmationMenuGUI menu = confirmScreen.GetComponent<ConfirmationMenuGUI>();
        if(menu != null)
        {
            menu.Initialise("Do you want to Restart current level?", RestartLevel, Pause);
        }
        pauseScreen.SetActive(false);
    }

    private void QuitToMenu()
    {
        Time.timeScale = 0;
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
