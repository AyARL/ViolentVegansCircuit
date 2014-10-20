using UnityEngine;
using System.Collections;

public class PauseMenuGUI : MonoBehaviour
{
    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
