using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class MainMenuGUI : MonoBehaviour
{
    public UnityAction OnContinueAvailable { get; set; }

    [SerializeField]
    private GameObject levelSelectScreen = null;

    [SerializeField]
    private GameObject rewardsScreen = null;

    private void Start()
    {
        if (SaveLoadFacilitator.Facilitator.HasSavedProgress())
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
        gameObject.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void Rewards()
    {
        gameObject.SetActive(false);
        rewardsScreen.SetActive(true);
    }

    public void Settings()
    {
        SaveLoadFacilitator.Facilitator.ResetProfile();
        LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.MainMenuIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
