using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Serialization.JsonFx;

[System.Serializable]
public class PlayerProfile
{
    public int LastLevelCompleted { get; set; }
    public List<LevelScore> LevelResults { get; set; }
    public bool MusicOn { get; set; }
    public bool SFXOn { get; set; }
    public bool VibrationOn { get; set; }
}

[System.Serializable]
public class LevelScore
{
    public int LevelIndex { get; set; }
    public int StarCount { get; set; }
}