using UnityEngine;
using System.Collections;
using System.Linq;
using Pathfinding.Serialization.JsonFx;
using Circuit;

public class GameController : MonoBehaviour
{
    public enum GameState { Game_Start, Game_Setup, Game_Play, Game_Win, Game_Fail }
    protected GameState gameState;

    [SerializeField]
    protected GameObject board = null;

    protected CircuitBoard circuitBoard = null;
    protected BoardFlowControl flowControl = null;

    [SerializeField]
    protected GameObject Player = null;
    [SerializeField]
    protected GameObject PlayerFace = null;

    protected bool WinConditionMet = false;
    protected bool FailConditionMet = false;

    protected int endPointsTotal = -1;
    protected int numberOfInactiveEndPoints = -1;

    protected int m_iMusicID;

    protected virtual void Start()
    {
        gameState = GameState.Game_Start;
        StartCoroutine(StateMachine());
    }

    // There isn't that much game logic, so a simple state machine should be enough
    protected virtual IEnumerator StateMachine()
    {
        while (true)
        {
            yield return StartCoroutine(gameState.ToString());
            yield return null;
        }
    }

    protected virtual IEnumerator Game_Start()
    {
        circuitBoard = board.GetComponent<CircuitBoard>();
        flowControl = board.GetComponent<BoardFlowControl>();

        WinConditionMet = false;
        FailConditionMet = false;

        gameState = GameState.Game_Setup;
        yield break;
    }

    protected virtual IEnumerator Game_Setup()
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

    protected virtual IEnumerator Game_Play()
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

    protected virtual IEnumerator Game_Win()
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

    protected virtual IEnumerator Game_Fail()
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

    protected void SetLevelStatus(bool levelWon)
    {
        int activatedChips = endPointsTotal - numberOfInactiveEndPoints;
        int awardedStars = CalculateAwardedStars(activatedChips);

        CompletedLevelStatus status = new CompletedLevelStatus()
        {
            LevelIndex = Application.loadedLevel,
            LevelWon = levelWon,
            ActivatedChips = activatedChips,
            MaxChips = endPointsTotal,
            StarsAwarded = awardedStars
        };

        SaveLoadFacilitator.Facilitator.SaveLevelResults(status);
    }

    protected int CalculateAwardedStars(int activatedChips)
    {
        if (activatedChips == 0)
        {
            return 0;
        }
        else if (activatedChips == endPointsTotal)
        {
            return 3;
        }
        else if (activatedChips == 1)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
