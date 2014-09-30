using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CircuitTilesSO))]
public class CircuitTilesSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUI.changed)
        {
            (target as CircuitTilesSO).UpdateTileMapping();
        }
    }
}
