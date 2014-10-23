using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBox : MenuBase
{
    [SerializeField]
    private Text levelNumberDisplay = null;

    [SerializeField]
    private GameObject[] stars = null;

    [SerializeField]
    private GameObject lockImage = null;

    private int levelNumber;

    public void Initialise(int levelNumber, bool unlocked, int starsEarned = 0)
    {
        if (unlocked)
        {
            this.levelNumberDisplay.text = levelNumber.ToString();
            if(starsEarned > 0)
            {
                DisplayStars(starsEarned);
            }
            lockImage.SetActive(false);

            this.levelNumber = levelNumber;
            GetComponent<Button>().onClick.AddListener(() => OnButtonPressed("LoadLevel"));
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void LoadLevel()
    {
        LoadingManager.LoadLevel(levelNumber + LoadingManager.LevelLoadingSettings.FirstGameLevel - 1);
    }

    private void DisplayStars(int count)
    {
        for(int i = 0; i < count; i++)
        {
            stars[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
