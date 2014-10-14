using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MainMenuGUI : MonoBehaviour
{
    [SerializeField]
    private int firstLevelIndex = 0;

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
        LoadingManager.LoadLevel(firstLevelIndex);
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
