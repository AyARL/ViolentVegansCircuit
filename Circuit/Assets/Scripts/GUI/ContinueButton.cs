using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().interactable = false;
        GetComponentInParent<MainMenuGUI>().OnContinueAvailable += Enable;

    }

    private void Enable()
    {
        GetComponentInParent<MainMenuGUI>().OnContinueAvailable -= Enable;
        GetComponent<Button>().interactable = true;
    }
}
