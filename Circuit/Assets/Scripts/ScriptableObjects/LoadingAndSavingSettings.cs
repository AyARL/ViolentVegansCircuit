using UnityEngine;
using System.Collections;

public class LoadingAndSavingSettings : ScriptableObject
{
    [SerializeField]
    private string levelStatus = ""; // Constins Serialized value of a CompletedLevelStatus object - created once level is completed (either win or fail)
    public string LevelStatus { get { return levelStatus; } }

    [SerializeField]
    private string savedProfile = ""; // Saved profile data with details on completed levels
    public string SavedProfile { get { return savedProfile; } }
}
