using UnityEngine;
using UnityEditor;
using System.Collections;

public static class CustomMenuItems
{
    [MenuItem("Circuit/Board/Create Board in Scene")]
    public static void CreateBoard()
    {
        GameObject board = new GameObject("CircuitBoard", typeof(CircuitBoard));
        board.GetComponent<CircuitBoard>().Initialise();
        board.AddComponent<BoardFlowControl>();
    }

    [MenuItem("Circuit/Board/Create Tile Prefab Resource")]
    public static void CreateTilePrefabResource()
    {
        ScriptableObjectUtility.CreateAsset<CircuitTilesSO>();
    }
}
