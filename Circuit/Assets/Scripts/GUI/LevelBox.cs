using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBox : MonoBehaviour
{
    [SerializeField]
    private Text levelNumber = null;

    [SerializeField]
    private GameObject[] stars = null;

    [SerializeField]
    private GameObject lockImage = null;

    public void Initialise(int levelNumber, bool unlocked, int starsEarned = 0)
    {
        if (unlocked)
        {
            this.levelNumber.text = levelNumber.ToString();
            if(starsEarned > 0)
            {
                DisplayStars(starsEarned);
            }
            lockImage.SetActive(false);

            GetComponent<Button>().onClick.AddListener(() => LoadingManager.LoadLevel(levelNumber + LoadingManager.LevelLoadingSettings.FirstGameLevel - 1));
        }
    }

    private void DisplayStars(int count)
    {
        for(int i = 0; i < count; i++)
        {
            stars[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
