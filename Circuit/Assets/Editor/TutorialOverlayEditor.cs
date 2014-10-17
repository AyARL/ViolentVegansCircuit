using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TutorialOverlay))]
public class TutorialOverlayEditor : Editor
{
    TutorialOverlay overlay;

    private void OnEnable()
    {
        overlay = target as TutorialOverlay;
        overlay.DetectOverlay();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Recalculate"))
        {
            overlay.Recalculate();
        }
    }

}
