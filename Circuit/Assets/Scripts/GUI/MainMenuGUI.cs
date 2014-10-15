using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class MainMenuGUI : MonoBehaviour
{

    private int lastLevelPlayed = -1;

    public UnityAction OnContinueAvailable { get; set; }

    private void Start()
    {
        if (SaveLoadFacilitator.Facilitator.GetLastLevelCompleted(out lastLevelPlayed))
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
        List<LevelScore> levelScores;
        if (SaveLoadFacilitator.Facilitator.GetProfileLevelResults(out levelScores))
        {
            if (levelScores.Count < (Application.levelCount - LoadingManager.LevelLoadingSettings.FirstGameLevel)) // Not all levels completed
            {
                int last = levelScores.Max(l => l.LevelIndex);
                LoadingManager.LoadLevel(last + 1);
            }
            else // All levels completed
            {
                var notAllStars = levelScores.Where(l => l.StarCount < 3).OrderBy(l => l.LevelIndex);
                LevelScore firstLow = notAllStars.FirstOrDefault();
                if (firstLow != null)
                {
                    LoadingManager.LoadLevel(firstLow.LevelIndex);
                }
                else
                {
                    LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.FirstGameLevel);
                }
            }
        }
    }

    public void SelectLevel()
    {
        List<LevelScore> levelScores;
        if (SaveLoadFacilitator.Facilitator.GetProfileLevelResults(out levelScores))
        {
            foreach (LevelScore score in levelScores)
            {
                Debug.Log(string.Format("Level: {0} Stars: {1}", score.LevelIndex, score.StarCount));
            }
        }
    }

    public void Settings()
    {
        SaveLoadFacilitator.Facilitator.ResetProfile();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
