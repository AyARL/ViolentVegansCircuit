﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class LevelCompletedGUI : MonoBehaviour
{
    [SerializeField]
    private GameObject starContainer = null;
    private Animation[] StarAnimations = null;

    [SerializeField]
    private GameObject winTitle = null;
    [SerializeField]
    private GameObject failTitle = null;

    CompletedLevelStatus status = null;

    int awardedStars = 0;

    // Use this for initialization
    void Start()
    {
        if (SaveLoadFacilitator.Facilitator.LoadLevelResults(out status))
        {
            if (status.LevelWon)
            {
                winTitle.SetActive(true);
                StarAnimations = starContainer.GetComponentsInChildren<Animation>();
                awardedStars = status.StarsAwarded;
                StartCoroutine(DelayStarDrop(0.5f, 0.2f));
            }
            else
            {
                failTitle.SetActive(true);
            }
        }
    }

    private IEnumerator DelayStarDrop(float time, float interval)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i < awardedStars; i++)
        {
            StarAnimations[i].Play();
            yield return new WaitForSeconds(interval);
        }
    }


    // Button functions
    public void ReplayLevel()
    {
        LoadingManager.LoadLevel(status.LevelIndex);
    }

    public void NextLevel()
    {
        if(status.LevelIndex < Application.levelCount - 1)
        {
            LoadingManager.LoadLevel(status.LevelIndex + 1);
        }
    }

    public void MainMenu()
    {
        LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.MainMenuIndex);
    }

}