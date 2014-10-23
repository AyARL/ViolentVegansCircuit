using UnityEngine;
using System.Collections;
using Circuit;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialGameController : GameController
{
    public enum TutorialState { Tutorial_None, Tutorial_Start, Tutorial_Play, Tutorial_Win, Tutorial_Fail, Tutorial_ImpulseLost }
    private TutorialState tutorialState = TutorialState.Tutorial_None;  // Use SetTutorialState to change this

    [SerializeField]
    private TutorialInstructionQueue tutorialInstructions = null;

    [SerializeField]
    private TutorialOverlay overlayScript = null;

    [SerializeField]
    private Text textField = null;

    private IEnumerator SetTutorialState(TutorialState newState)
    {
        tutorialState = newState;
        var instructions = tutorialInstructions.GetInstructionsForState(tutorialState);
        if (instructions != null)
        {
            foreach (TutorialInstruction instruction in instructions)
            {
                yield return StartCoroutine(ScaleTime(instruction.TimeScale, 5f));

                if (overlayScript != null && instruction.UseOverlay)
                {
                    overlayScript.gameObject.SetActive(true);
                    overlayScript.SetOverlayHighlights(instruction.OverlayTilesToHighlight);
                }

                textField.text = instruction.InstructionText;

                while (true)
                {
                    // If there is input NOT on UI elements
                    if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        break;
                    }
                    else
                    {
                        yield return null;
                    }
                }


                if (overlayScript != null)
                {
                    overlayScript.gameObject.SetActive(false);
                }

                textField.text = "";
                Time.timeScale = 1f;
            }
        }

        yield break;
    }

    protected override void Start()
    {
        base.Start();

        if (tutorialInstructions != null)
        {
            tutorialInstructions.Initialise();
        }

        if (overlayScript != null)
        {
            overlayScript.gameObject.SetActive(false);
        }
    }

    protected override IEnumerator StateMachine()
    {
        while (true)
        {
            runningState = StartCoroutine(gameState.ToString());
            yield return runningState;
            yield return null;
        }
    }

    protected override IEnumerator Game_Start()
    {
        circuitBoard = board.GetComponent<CircuitBoard>();
        flowControl = board.GetComponent<BoardFlowControl>();

        WinConditionMet = false;
        FailConditionMet = false;

        gameState = GameState.Game_Setup;
        yield break;
    }

    protected override IEnumerator Game_Setup()
    {
        // Start up the music.
        if (m_iMusicID <= 0)
            m_iMusicID = CAudioControl.CreateAndPlayAudio(CAudio.AUDIO_MUSIC, true, true, true, 0.3f);

        flowControl.OnImpulseRemoved += ImpulseLost;
        flowControl.OnEndPointActivated += EndPointActivated;

        endPointsTotal = circuitBoard.GetEndTilesCount();
        numberOfInactiveEndPoints = endPointsTotal;

        // Spawn tiles
        var tileOrderIndices = Enumerable.Range(0, circuitBoard.Tiles.Count).ToList();
        Utility.Shuffle(tileOrderIndices);


        foreach (int i in tileOrderIndices)
        {
            circuitBoard.Tiles[i].GetComponentInChildren<Animator>().SetTrigger("FallIn");
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        PlayerFace.SetActive(true);
        Player.SetActive(true);

        yield return StartCoroutine(SetTutorialState(TutorialState.Tutorial_Start));
        yield return new WaitForSeconds(1f);

        flowControl.SpawnImpulse();
        flowControl.RunImpulses();

        gameState = GameState.Game_Play;
        yield break;
    }

    protected override IEnumerator Game_Play()
    {
        yield return StartCoroutine(SetTutorialState(TutorialState.Tutorial_Play));

        inGameGUI.gameObject.SetActive(true);

        while (gameState == GameState.Game_Play)
        {
            yield return null;

            if (WinConditionMet)
            {
                gameState = GameState.Game_Win;
                inGameGUI.OnPauseGame -= PauseGame;
                inGameGUI.gameObject.SetActive(false);
                yield break;
            }

            if (FailConditionMet)
            {
                gameState = GameState.Game_Fail;
                inGameGUI.OnPauseGame -= PauseGame;
                inGameGUI.gameObject.SetActive(false);
                yield break;
            }
        }
    }

    protected override IEnumerator Game_Win()
    {
        // Stop the music.
        CAudioControl.StopSound(m_iMusicID);
        m_iMusicID = 0;

        Handheld.Vibrate();

        flowControl.OnImpulseRemoved -= ImpulseLost;
        flowControl.OnEndPointActivated -= EndPointActivated;

        if (endLevelSoundID <= 0)
        {
            endLevelSoundID = CAudioControl.CreateAndPlayAudio(CAudio.AUDIO_EFFECT_LEVEL_COMPLETED, false, true, false, 1f);
        }

        yield return StartCoroutine(PlayEffects(winEffect, 0.1f));

        CAudioControl.StopSound(endLevelSoundID);
        CAudioControl.ClearContainers();

        SetLevelStatus(true);

        yield return StartCoroutine(SetTutorialState(TutorialState.Tutorial_Win));

        if (LoadingManager.LevelLoadingSettings != null)
        {
            LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.LevelEndScreen);
        }
        yield break;
    }

    protected override IEnumerator Game_Fail()
    {
        // Stop the music.
        CAudioControl.StopSound(m_iMusicID);
        m_iMusicID = 0;

        Handheld.Vibrate();

        flowControl.OnImpulseRemoved -= ImpulseLost;
        flowControl.OnEndPointActivated -= EndPointActivated;

        yield return StartCoroutine(PlayEffects(failEffect, 0f));

        if (endLevelSoundID <= 0)
        {
            endLevelSoundID = CAudioControl.CreateAndPlayAudio(CAudio.AUDIO_EFFECT_GAMEOVER, false, true, false, 1f);
        }

        var tileOrderIndices = Enumerable.Range(0, circuitBoard.Tiles.Count).ToList();
        Utility.Shuffle(tileOrderIndices);


        yield return StartCoroutine(SetTutorialState(TutorialState.Tutorial_Fail));

        foreach (int i in tileOrderIndices)
        {
            circuitBoard.Tiles[i].GetComponentInChildren<Animator>().SetTrigger("FallOut");
            yield return new WaitForSeconds(0.05f);
        }

        CAudioControl.StopSound(endLevelSoundID);
        CAudioControl.ClearContainers();

        SetLevelStatus(false);

        if (LoadingManager.LevelLoadingSettings != null)
        {
            LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.LevelEndScreen);
        }
    }

    private IEnumerator ScaleTime(float targetScale, float scalingTime)
    {
        float speed = Mathf.Abs(Time.timeScale - targetScale) / scalingTime;
        float t = 0;
        float deltaTime = Time.fixedDeltaTime;

        while (Mathf.Abs(Time.timeScale - targetScale) > 0.1f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetScale, t);
            t += speed * deltaTime;
            yield return null;
        }
        Time.timeScale = targetScale;
    }
}
