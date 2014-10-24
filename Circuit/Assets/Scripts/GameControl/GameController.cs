using UnityEngine;
using System.Collections;
using System.Linq;
using Pathfinding.Serialization.JsonFx;
using Circuit;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public enum GameState { Game_Start, Game_Setup, Game_Play, Game_Win, Game_Fail, Game_Pause }
    protected GameState gameState;
    protected Coroutine runningState; 

    [SerializeField]
    protected GameObject board = null;

    protected CircuitBoard circuitBoard = null;
    protected BoardFlowControl flowControl = null;

    [SerializeField]
    protected GameObject Player = null;
    [SerializeField]
    protected GameObject PlayerFace = null;

    protected InGameGUI inGameGUI = null;

    protected bool WinConditionMet = false;
    protected bool FailConditionMet = false;

    protected int endPointsTotal = -1;
    protected int numberOfInactiveEndPoints = -1;

    protected int m_iMusicID;
    protected int endLevelSoundID;

    [SerializeField]
    protected Color defaultBgColour = new Color(0f, 0f, 0f);

    [SerializeField]
    protected GameObject winEffect = null;
    [SerializeField]
    protected GameObject failEffect = null;

    protected float gameTimescale = 1f;

    protected virtual void Start()
    {
        inGameGUI = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<InGameGUI>();

        inGameGUI.OnPauseGame += PauseGame;
        inGameGUI.gameObject.SetActive(false);

        gameState = GameState.Game_Start;
        StartCoroutine(StateMachine());
    }

    // There isn't that much game logic, so a simple state machine should be enough
    protected virtual IEnumerator StateMachine()
    {
        while (true)
        {
            runningState = StartCoroutine(gameState.ToString());
            yield return runningState;
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

        // Spawn Impulse
        flowControl.SpawnImpulse();
        flowControl.RunImpulses();

        yield break;
    }

    protected virtual IEnumerator Game_Play()
    {
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

    protected virtual IEnumerator Game_Pause()
    {
        gameTimescale = Time.timeScale;
        Time.timeScale = 0f;

        inGameGUI.OnResumeGame += ResumeGame;

        while(gameState == GameState.Game_Pause)
        {
            yield return null;
        }

        inGameGUI.OnResumeGame -= ResumeGame;
    }

    protected virtual IEnumerator Game_Win()
    {
        // Stop the music.
        CAudioControl.StopSound(m_iMusicID);
        m_iMusicID = 0;

        Handheld.Vibrate();

        flowControl.OnImpulseRemoved -= ImpulseLost;
        flowControl.OnEndPointActivated -= EndPointActivated;

        if(endLevelSoundID <= 0)
        {
            endLevelSoundID = CAudioControl.CreateAndPlayAudio(CAudio.AUDIO_EFFECT_LEVEL_COMPLETED, false, true, false, 1f);
        }

        yield return StartCoroutine(PlayEffects(winEffect, 0.1f));

        CAudioControl.StopSound(endLevelSoundID);
        CAudioControl.ClearContainers();

        SetLevelStatus(true);
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

    protected void EndPointActivated()
    {
        StartCoroutine(FlashBackgroundColor(Color.cyan));

        numberOfInactiveEndPoints -= 1;
        if (numberOfInactiveEndPoints == 0)
        {
            WinConditionMet = true;
        }
    }

    protected void ImpulseLost(int impulsesLeft, bool lost)
    {
        if (lost)
        {
            StartCoroutine(FlashBackgroundColor(Color.red));
        }

        if (impulsesLeft == 0)
        {
            if (numberOfInactiveEndPoints == endPointsTotal)
            {
                CBall ball = Player.GetComponent< CBall >();
                if ( null == ball )
                {
                    Debug.LogError( string.Format( "{0}: {1}", CErrorStrings.ERROR_MISSING_COMPONENT, typeof( CBall ).ToString() ) );
                }
                else
                {
                    ball.SetBallState( CBall.EBallState.STATE_UNHAPPY );
                }

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

    protected IEnumerator FlashBackgroundColor(Color color)
    {
        float speed = 1f;
        float t = 0f;
        Camera.main.backgroundColor = color;

        while(Camera.main.backgroundColor != defaultBgColour)
        {
            Camera.main.backgroundColor = Color.Lerp(color, defaultBgColour, t);
            t += speed * Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    protected IEnumerator PlayEffects(GameObject container, float delay)
    {
        var particleSystems = container.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
        }
    }

    protected virtual void PauseGame()
    {
        if (gameState == GameState.Game_Play)
        {
            gameState = GameState.Game_Pause;
        }
    }

    protected virtual void ResumeGame()
    {
        if(gameState == GameState.Game_Pause)
        {
            gameState = GameState.Game_Play;
            Time.timeScale = gameTimescale;
        }
    }
}
