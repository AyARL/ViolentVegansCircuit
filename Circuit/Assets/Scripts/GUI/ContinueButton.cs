using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueButton : MonoBehaviour
{
    MainMenuGUI mainMenu = null;

    private void Awake()
    {
        mainMenu = GetComponentInParent<MainMenuGUI>();

        mainMenu.OnContinueAvailable += SetToContinue;
        GetComponentInChildren<Text>().text = "New Game";
        GetComponent<Button>().onClick.AddListener(() => mainMenu.OnButtonPressed("StartNewGame"));
    }

    private void SetToContinue()
    {
        GetComponentInParent<MainMenuGUI>().OnContinueAvailable -= SetToContinue;
        GetComponentInChildren<Text>().text = "Continue";
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => mainMenu.OnButtonPressed("ContinueGame"));
    }
}
