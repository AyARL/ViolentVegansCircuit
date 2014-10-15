using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class LevelCompletedGUI : MonoBehaviour
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

    int awardedStars = 0;

    // Use this for initialization
    void Start()
    {
        if (SaveLoadFacilitator.Facilitator.LoadLevelResults(out status))
        {
            activeChipsCounter.text = status.ActivatedChips.ToString();
            totalChipsCounter.text = status.MaxChips.ToString();

            if (status.LevelWon)
            {
                winTitle.SetActive(true);
                StarAnimations = starContainer.GetComponentsInChildren<Animation>();
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
            StarAnimations[i].Play();
            yield return new WaitForSeconds(interval);
        }
    }


    // Button functions
    public void ReplayLevel()
    {
        LoadingManager.LoadLevel(status.LevelIndex);
    }

    public void NextLevel()
    {
        LoadingManager.LoadLevel(status.LevelIndex + 1);
    }

    public void MainMenu()
    {
        LoadingManager.LoadLevel(LoadingManager.LevelLoadingSettings.MainMenuIndex);
    }

}
