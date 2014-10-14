using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MainMenuGUI : MonoBehaviour
{

    private int lastLevelPlayed = -1;

    public UnityAction OnContinueAvailable { get; set; }

    private void Start()
    {
        lastLevelPlayed = PlayerPrefs.GetInt("LastLevelPlayed", -1);
        if (lastLevelPlayed > 0)
        {
            OnContinueAvailable();
        }
    }

    public void StartNewGame()
    {
        if (LoadingManager.LevelLoadingSettings != null)
        {
            LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.FirstGameLevel);
        }
    }

    public void ContinueGame()
    {

    }

    public void SelectLevel()
    {

    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
