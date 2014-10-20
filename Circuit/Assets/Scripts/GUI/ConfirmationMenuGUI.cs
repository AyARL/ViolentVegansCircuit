using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmationMenuGUI : MonoBehaviour
{
    [SerializeField]
    private Text titleMessage = null;

    private UnityAction confirmAction  = null;
    private UnityAction backAction = null;

    public void Initialise(string message, UnityAction confirmation, UnityAction back)
    {
        titleMessage.text = message;
        confirmAction = confirmation;
        backAction = back;
    }

    public void Confirmed()
    {
        if (confirmAction != null)
        {
            confirmAction();
        }
    }

    public void Back()
    {
        gameObject.SetActive(false);
        if (backAction != null)
        {
            backAction();
        }
    }
}
