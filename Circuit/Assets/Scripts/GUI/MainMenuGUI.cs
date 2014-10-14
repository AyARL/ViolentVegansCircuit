using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MainMenuGUI : MonoBehaviour
{

    private int lastLevelPlayed = -1;

    public UnityAction OnContinueAvailable { get; set; }

    private void Start()
    {
        if(SaveLoadFacilitator.Facilitator.GetLastLevelCompleted(out lastLevelPlayed))
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
        if (lastLevelPlayed < Application.levelCount - 1)
        {
            LoadingManager.LoadLevel(lastLevelPlayed + 1);
        }
    }

    public void SelectLevel()
    {
        List<LevelScore> levelScores;
        if(SaveLoadFacilitator.Facilitator.GetProfileLevelResults(out levelScores))
        {
            foreach(LevelScore score in levelScores)
            {
                Debug.Log(string.Format("Level: {0} Stars: {1}", score.LevelIndex, score.StarCount));
            }
        }
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
