using UnityEngine;
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

    CompletedLevelStatus status = null;

    int awardedStars = 0;

    // Use this for initialization
    void Start()
    {
        string statusString = PlayerPrefs.GetString("LevelStatus");
        status = Utility.ValidateJsonData<CompletedLevelStatus>(statusString);
        if (status != default(CompletedLevelStatus))
        {
            if (status.LevelWon)
            {
                winTitle.SetActive(true);
                StarAnimations = starContainer.GetComponentsInChildren<Animation>();
                awardedStars = CalculateAwardedStars();
                StartCoroutine(DelayStarDrop(0.5f, 0.2f));
            }
            else
            {
                failTitle.SetActive(true);

            }
        }
    }

    private IEnumerator DelayStarDrop(float time, float interval)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i < awardedStars; i++)
        {
            StarAnimations[i].Play();
            yield return new WaitForSeconds(interval);
        }
    }

    private int CalculateAwardedStars()
    {
        if (status.ActivatedChips == status.MaxChips)
        {
            return 3;
        }
        else if (status.ActivatedChips == 1)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

}
