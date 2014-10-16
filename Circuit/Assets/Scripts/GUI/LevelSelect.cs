using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject levelBoxPrefab = null;
    [SerializeField]
    private GameObject levelGrid = null;

    [SerializeField]
    private GameObject mainMenuScreen = null;
    [SerializeField]
    private GameObject backButton = null;

    // Use this for initialization
    void Start()
    {
        //Get total level count
        int levelCount = Application.levelCount - LoadingManager.LevelLoadingSettings.FirstGameLevel;

        //Attempt to load profile data
        List<LevelScore> levelScores;
        if (SaveLoadFacilitator.Facilitator.GetProfileLevelResults(out levelScores))
        {
            for (int i = 0; i < levelCount; i++)
            {
                LevelScore score = levelScores.FirstOrDefault(l => l.LevelIndex == i + LoadingManager.LevelLoadingSettings.FirstGameLevel);
                if (score != null)
                {
                    CreateLevelBox(true, i + 1, score.StarCount);
                }
                else
                {
                    bool unlocked = levelScores.Any(l => l.LevelIndex == (i + LoadingManager.LevelLoadingSettings.FirstGameLevel) - 1);
                    CreateLevelBox(unlocked, i + 1);
                }
            }
        }
        else // Display only first level as unlocked
        {
            CreateLevelBox(true, 1);

            for(int i = 1; i < levelCount; i++)
            {
                CreateLevelBox(false, i + 1);
            }
        }
    }

    void OnEnable()
    {
        backButton.SetActive(true);
        backButton.GetComponent<Button>().onClick.AddListener(() => Exit());
    }

    private void CreateLevelBox(bool unlocked, int levelNumber, int starCount = 0)
    {
        GameObject levelBox = Instantiate(levelBoxPrefab) as GameObject;
        levelBox.transform.SetParent(levelGrid.transform, false);
        levelBox.GetComponent<LevelBox>().Initialise(levelNumber, unlocked, starCount);
    }


    private void Exit()
    {
        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backButton.SetActive(false);
        gameObject.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
