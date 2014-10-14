﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public static class CustomMenuItems
{
    [MenuItem("Arc Flash/Board/Create Board in Scene")]
    public static void CreateBoard()
    {
        GameObject board = new GameObject("CircuitBoard", typeof(CircuitBoard));
        board.GetComponent<CircuitBoard>().Initialise();
        board.AddComponent<BoardFlowControl>();
    }

    [MenuItem("Arc Flash/Board/Create Tile Prefab Resource")]
    public static void CreateTilePrefabResource()
    {
        ScriptableObjectUtility.CreateAsset<CircuitTilesSO>();
    }

    [MenuItem("Arc Flash/Settings/Level Indices")]
    public static void CreateLevelSettingsSO()
    {
        ScriptableObjectUtility.CreateResource<LevelLoadingSettings>();
    }
}
