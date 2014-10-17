using UnityEngine;
using System.Collections;
using Circuit;
using System.Linq;

public class TutorialGameController : GameController
{
    public enum TutorialState { Tutorial_None, Tutorial_Start, Tutorial_Play, Tutorial_Tether, Tutorial_Win, Tutorial_Fail }
    private TutorialState tutorialState = TutorialState.Tutorial_None;

    [SerializeField]
    private TutorialInstructionQueue tutorialInstructions = null;

    protected IEnumerator StateMachine()
    {
        while (true)
        {
            yield return StartCoroutine(gameState.ToString());
            yield return null;
        }
    }

    protected IEnumerator Game_Start()
    {
        circuitBoard = board.GetComponent<CircuitBoard>();
        flowControl = board.GetComponent<BoardFlowControl>();

        WinConditionMet = false;
        FailConditionMet = false;

        gameState = GameState.Game_Setup;
        yield break;
    }

    protected IEnumerator Game_Setup()
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

        yield return new WaitForSeconds(1f);

        gameState = GameState.Game_Play;
        yield break;
    }

    protected IEnumerator Game_Play()
    {
        // Spawn Impulse
        flowControl.SpawnImpulse();
        flowControl.RunImpulses();

        while (true)
        {
            yield return null;

            if (WinConditionMet)
            {
                gameState = GameState.Game_Win;
                yield break;
            }

            if (FailConditionMet)
            {
                gameState = GameState.Game_Fail;
                yield break;
            }
        }
    }

    protected IEnumerator Game_Win()
    {
        // Stop the music.
        CAudioControl.StopSound(m_iMusicID);
        m_iMusicID = 0;

        CAudioControl.ClearContainers();

        Handheld.Vibrate();

        flowControl.OnImpulseRemoved -= ImpulseLost;
        flowControl.OnEndPointActivated -= EndPointActivated;

        Debug.Log("Win!");

        SetLevelStatus(true);
        yield return new WaitForSeconds(1f);
        if (LoadingManager.LevelLoadingSettings != null)
        {
            LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.LevelEndScreen);
        }
        yield break;
    }

    protected IEnumerator Game_Fail()
    {
        // Stop the music.
        CAudioControl.StopSound(m_iMusicID);
        m_iMusicID = 0;

        CAudioControl.ClearContainers();

        Handheld.Vibrate();

        flowControl.OnImpulseRemoved -= ImpulseLost;
        flowControl.OnEndPointActivated -= EndPointActivated;

        var tileOrderIndices = Enumerable.Range(0, circuitBoard.Tiles.Count).ToList();
        Utility.Shuffle(tileOrderIndices);

        foreach (int i in tileOrderIndices)
        {
            circuitBoard.Tiles[i].GetComponentInChildren<Animator>().SetTrigger("FallOut");
            yield return new WaitForSeconds(0.05f);
        }

        SetLevelStatus(false);
        if (LoadingManager.LevelLoadingSettings != null)
        {
            LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.LevelEndScreen);
        }
    }

    protected void EndPointActivated()
    {
        numberOfInactiveEndPoints -= 1;
        if (numberOfInactiveEndPoints == 0)
        {
            WinConditionMet = true;
        }
    }

    protected void ImpulseLost(int impulsesLeft)
    {
        if (impulsesLeft == 0)
        {
            if (numberOfInactiveEndPoints == endPointsTotal)
            {
                FailConditionMet = true;
            }
            else
            {
                WinConditionMet = true;
            }
        }
    }

}
