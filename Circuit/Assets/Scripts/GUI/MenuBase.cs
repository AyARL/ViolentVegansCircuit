using UnityEngine;
using System.Collections;
using Circuit;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class MenuBase : MonoBehaviour
{
    private int buttonSoundId;

    protected GameObject[] buttonsInScene;

    public void OnButtonPressed(string action)
    {
        StartCoroutine(ButtonPressed(action));
    }

    private IEnumerator ButtonPressed(string action)
    {
        if (buttonSoundId <= 0)
        {
            buttonSoundId = CAudioControl.CreateAndPlayAudio(CAudio.AUDIO_EFFECT_MENU_SELECT, false, true, false, 1f);
        }

        yield return null;

        Invoke(action, 0f);
        CAudioControl.StopSound(buttonSoundId, false);
    }

    private void DisableAllButtons()
    {
        if (buttonsInScene == null)
        {
            buttonsInScene = GameObject.FindGameObjectsWithTag("Button");
        }

        foreach (Button button in buttonsInScene.Select(b => b.GetComponent<Button>()))
        {
            button.interactable = false;
        }
    }

    private void EnableAllButtons()
    {
        if (buttonsInScene == null)
        {
            buttonsInScene = GameObject.FindGameObjectsWithTag("Button");
        }

        foreach (Button button in buttonsInScene.Select(b => b.GetComponent<Button>()))
        {
            button.interactable = true;
        }
    }

}
