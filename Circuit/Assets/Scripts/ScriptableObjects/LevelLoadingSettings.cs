using UnityEngine;
using System.Collections;

public class LevelLoadingSettings : ScriptableObject
{
    [SerializeField]
    private int mainMenuIndex = 0;
    public int MainMenuIndex { get { return mainMenuIndex; } }

    [SerializeField]
    private int loadingScreenIndex = 0;
    public int LoadingScreenIndex { get { return loadingScreenIndex; } }

    [SerializeField]
    private int levelEndScreen = 0;
    public int LevelEndScreen { get { return levelEndScreen; } }

    [SerializeField]
    private int firstGameLevel = 0;
    public int FirstGameLevel { get { return firstGameLevel; } }
}
