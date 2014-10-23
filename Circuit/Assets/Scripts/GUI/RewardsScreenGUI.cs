using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RewardsScreenGUI : MenuBase
{
    [SerializeField]
    private GameObject backButton = null;

    //[SerializeField]
    //private GameObject grid = null;

    [SerializeField]
    private GameObject mainMenuScreen = null;

    void OnEnable()
    {
        backButton.SetActive(true);
        backButton.GetComponent<Button>().onClick.AddListener(() => Exit());
    }

    private void Exit()
    {
        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backButton.SetActive(false);
        gameObject.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
}
