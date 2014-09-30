using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CircuitBoard))]
public class CircuitBoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIStyle customButton = new GUIStyle("button") { fontSize = 25 };



        GUILayout.BeginHorizontal();
        if (GUILayout.Button("↶", customButton))
        {
            
        }
        
        if (GUILayout.Button("↷", customButton))
        {
            
        }
        GUILayout.EndHorizontal();
    }
}
