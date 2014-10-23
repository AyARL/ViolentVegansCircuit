using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Circuit;

public class LevelCompletedGUI : MenuBase
{
    [SerializeField]
    private GameObject starContainer = null;
    private Animation[] StarAnimations = null;

    [SerializeField]
    private GameObject winTitle = null;
    [SerializeField]
    private GameObject failTitle = null;

    [SerializeField]
    private GameObject nextButton = null;

    [SerializeField]
    private Text activeChipsCounter = null;
    [SerializeField]
    private Text totalChipsCounter = null;

    CompletedLevelStatus status = null;

    private int awardedStars = 0;

    private int starAudioID;

    // Use this for initialization
    void Start()
    {
        if (SaveLoadFacilitator.Facilitator.LoadLevelResults(out status))
        {
            activeChipsCounter.text = status.ActivatedChips.ToString();
            totalChipsCounter.text = status.MaxChips.ToString();

            StarAnimations = starContainer.GetComponentsInChildren<Animation>();
            foreach (Animation anim in StarAnimations)
            {
                anim.gameObject.SetActive(false);
            }

            if (status.LevelWon)
            {
                winTitle.SetActive(true);
                
                awardedStars = status.StarsAwarded;
                StartCoroutine(DelayStarDrop(0.5f, 0.2f));

                if (status.LevelIndex < Application.levelCount - 1)
                {
                    nextButton.SetActive(true);
                }
                else
                {
                    nextButton.SetActive(false);
                }
            }
            else
            {
                failTitle.SetActive(true);
                nextButton.SetActive(false);
            }
        }
    }

    private IEnumerator DelayStarDrop(float time, float interval)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < awardedStars; i++)
        {
            StarAnimations[i].gameObject.SetActive(true);
            StarAnimations[i].Play();
            yield return new WaitForSeconds(interval);

            if(starAudioID > 0)
            {
                CAudioControl.StopSound(starAudioID);
            }
            starAudioID = CAudioControl.CreateAndPlayAudio(CAudio.AUDIO_EFFECT_STAR_FALL, false, true, false, 1f);
        }
    }


    // Button functions
    public void ReplayLevel()
    {
        CAudioControl.StopSound(starAudioID);
        CAudioControl.ClearContainers();
        LoadingManager.LoadLevel(status.LevelIndex);
    }

    public void NextLevel()
    {
        CAudioControl.StopSound(starAudioID);
        CAudioControl.ClearContainers();
        LoadingManager.LoadLevel(status.LevelIndex + 1);
    }

    public void MainMenu()
    {
        CAudioControl.StopSound(starAudioID);
        CAudioControl.ClearContainers();
        LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.MainMenuIndex);
    }

}
