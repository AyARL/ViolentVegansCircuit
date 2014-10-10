using UnityEngine;
using System.Collections;
using Pathfinding.Serialization.JsonFx;

public class Temp_LevelEndGUI : MonoBehaviour
{
    CompletedLevelStatus status = null;

    private bool displayData = false;

    // Use this for initialization
    void Start()
    {
        string statusString = PlayerPrefs.GetString("LevelStatus");
        status = ValidateJsonData<CompletedLevelStatus>(statusString);
        if(status != default(CompletedLevelStatus))
        {
            displayData = true;
        }
    }

    private void OnGUI()
    {
        if(displayData)
        {
            GUILayout.BeginVertical("Level Results:");
            GUILayout.Label(string.Format("Level {0}!", status.LevelWon ? "Completed" : "Failed"));
            GUILayout.Label(string.Format("{0}/{1} Chips Powered", status.ActivatedChips, status.MaxChips));
        }
    }

    private T ValidateJsonData<T>(string input)
    {
        T output;
        try
        {
            output = JsonReader.Deserialize<T>(input);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            output = default(T);
        }

        return output;
    }

  
}
