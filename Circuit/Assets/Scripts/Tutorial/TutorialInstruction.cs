using UnityEngine;
using System.Collections;

public class TutorialInstruction : ScriptableObject
{
    [SerializeField]
    private float timeScale = 0;
    public float TimeScale { get { return timeScale; } }

    [SerializeField]
    private string instructionText = null;
    public string InstructionText { get { return instructionText; } }

    [SerializeField]
    private bool useOverlay = false;
    public bool UseOverlay { get { return useOverlay; } }

    [SerializeField]
    private Vector2Int[] overlayTilesToHighlight = null;
    public Vector2Int[] OverlayTilesToHighlight { get { return overlayTilesToHighlight; } }

    [SerializeField]
    private TutorialGameController.TutorialState triggerState = TutorialGameController.TutorialState.Tutorial_None;
    public TutorialGameController.TutorialState TriggerState { get { return triggerState; } }

}

[System.Serializable]
public struct Vector2Int
{
#pragma warning disable 414
    [SerializeField]
    private int x;
    public int X { get { return x; } }

    [SerializeField]
    private int y;
    public int Y { get { return y; } }
#pragma warning restore 414
}