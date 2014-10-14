using UnityEngine;
using System.Collections;

[System.Serializable]
public class CompletedLevelStatus
{
    public int LevelIndex { get; set; }
    public bool LevelWon { get; set; }
    public int ActivatedChips { get; set; }
    public int MaxChips { get; set; }
    public int StarsAwarded { get; set; }
}
