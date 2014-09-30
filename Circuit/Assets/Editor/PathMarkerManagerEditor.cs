using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PathMarkerManager))]
public class PathMarkerManagerEditor : Editor
{
    SerializedProperty markerType;

    private void OnEnable()
    {
        markerType = serializedObject.FindProperty("markerType");
        if (markerType == null)
        {
            Debug.LogError("markerType property not found");
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(markerType);

        (target as PathMarkerManager).UpdateToMatchType();
        serializedObject.ApplyModifiedProperties();
    }
}
