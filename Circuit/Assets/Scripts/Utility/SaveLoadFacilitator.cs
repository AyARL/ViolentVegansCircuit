using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;
using System.Linq;

public class SaveLoadFacilitator : MonoBehaviour
{
    private static SaveLoadFacilitator facilitator = null;
    public static SaveLoadFacilitator Facilitator { get { return facilitator; } }

    private static LoadingAndSavingSettings playerPrefsSettings = null;
    //public static LoadingAndSavingSettings PlayerPrefsSettings { get { return playerPrefsSettings; } }

    private PlayerProfile playerProfile = null;

    void Awake()
    {
        //PlayerPrefs.DeleteAll(); // Uncomment this to remove all saved data on start

        if (facilitator == null)
        {
            DontDestroyOnLoad(gameObject);
            facilitator = this;

            playerPrefsSettings = Resources.Load<LoadingAndSavingSettings>("PlayerPrefsIdentifiers");
            if (playerPrefsSettings == null)
            {
                Debug.LogException(new System.NullReferenceException("Could not load PlayerPrefsIdentifiers"));
            }

            LoadProfile();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateProfile()
    {
        if (playerProfile == null)
        {
            playerProfile = new PlayerProfile()
            {
                LastLevelCompleted = -1,
                LevelResults = new List<LevelScore>(),
                MusicOn = true,
                SFXOn = true,
                VibrationOn = true
            };
        }
    }

    private bool LoadProfile()
    {
        string serialisedProfile = PlayerPrefs.GetString(playerPrefsSettings.SavedProfile);
        if (serialisedProfile != "")
        {
            playerProfile = Utility.ValidateJsonData<PlayerProfile>(serialisedProfile);
            if (playerProfile != default(PlayerProfile))
            {
                return true;
            }
        }

        return false;
    }

    private void SaveProfile()
    {
        string serialisedProfile = JsonWriter.Serialize(playerProfile);
        PlayerPrefs.SetString(playerPrefsSettings.SavedProfile, serialisedProfile);
        PlayerPrefs.Save();
    }

    private void AddLastLevelCompletedToProfile(int levelIndex)
    {
        if (playerProfile == null)
        {
            CreateProfile();
        }

        playerProfile.LastLevelCompleted = levelIndex;
    }

    public bool GetLastLevelCompleted(out int levelIndex)
    {
        levelIndex = -1;

        if (playerProfile != null && playerProfile.LastLevelCompleted != -1)
        {
            levelIndex = playerProfile.LastLevelCompleted;
            return true;
        }

        return false;
    }

    private void UpdateProfileLevelResults(CompletedLevelStatus result)
    {
        if (playerProfile == null)
        {
            CreateProfile();
        }
        else
        {
            LevelScore levelScore = playerProfile.LevelResults.FirstOrDefault(s => s.LevelIndex == result.LevelIndex);
            if (levelScore != null)
            {
                if (levelScore.StarCount < result.StarsAwarded)
                {
                    levelScore.StarCount = result.StarsAwarded;
                }
            }
            else
            {
                playerProfile.LevelResults.Add(new LevelScore() { LevelIndex = result.LevelIndex, StarCount = result.StarsAwarded });
            }
        }
    }

    public bool GetProfileLevelResults(out List<LevelScore> output)
    {
        if (playerProfile != null && playerProfile.LevelResults.Count > 0)
        {
            output = playerProfile.LevelResults;
            return true;
        }

        output = null;
        return false;
    }

    public void SaveLevelResults(CompletedLevelStatus result)
    {
        string serialisedResult = JsonWriter.Serialize(result);
        PlayerPrefs.SetString(playerPrefsSettings.LevelStatus, serialisedResult);
        if (result.LevelWon)
        {
            AddLastLevelCompletedToProfile(result.LevelIndex);
            UpdateProfileLevelResults(result);
            SaveProfile();
        }
    }

    public bool LoadLevelResults(out CompletedLevelStatus result)
    {
        string statusString = PlayerPrefs.GetString("LevelStatus");
        CompletedLevelStatus status = Utility.ValidateJsonData<CompletedLevelStatus>(statusString);
        if (status != default(CompletedLevelStatus))
        {
            result = status;
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}
