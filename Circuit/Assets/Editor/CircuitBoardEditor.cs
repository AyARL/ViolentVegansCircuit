using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CircuitBoard))]
public class CircuitBoardEditor : Editor
{
    SerializedProperty tileList = null;
    int boardWidth = 0;
    int boardHeight = 0;

    bool[] foldouts;

    private void OnEnable()
    {
        GetTileList();
    }

    private bool GetTileList()
    {
        tileList = serializedObject.FindProperty("tiles");
        if (tileList == null)
        {
            Debug.LogError("Failed to retrieve tile list from Circuit Board");
            return false;
        }

        boardWidth = serializedObject.FindProperty("width").intValue;
        boardHeight = serializedObject.FindProperty("height").intValue;

        foldouts = new bool[boardWidth];

        return true;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (tileList == null)
        {
            if (!GetTileList())
            {
                return;
            }
        }

        GUIStyle customButton = new GUIStyle("button") { fontSize = 25 };

        for (int x = 0; x < boardWidth; x++)
        {
            foldouts[x] = EditorGUILayout.Foldout(foldouts[x], string.Format("Column {0}", x.ToString()));
            if (foldouts[x])
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    GUILayout.BeginHorizontal();

                    SerializedProperty tile = tileList.GetArrayElementAtIndex(boardWidth * y + x);
                    CircuitTile obj = tile.objectReferenceValue as CircuitTile;

                    GUILayout.Label(obj.name);

                    CircuitTile.CircuitTileType newType = (CircuitTile.CircuitTileType)EditorGUILayout.EnumPopup(obj.TileType, GUILayout.Width(100f));

                    if (newType != obj.TileType)
                    {
                        obj.SpawnTileOfType(newType);
                    }


                    if (GUILayout.Button("↶", customButton, GUILayout.Width(50f)))
                    {
                        obj.RotateCounterClockwise();
                    }

                    if (GUILayout.Button("↷", customButton, GUILayout.Width(50f)))
                    {
                        obj.RotateClockwise();
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
