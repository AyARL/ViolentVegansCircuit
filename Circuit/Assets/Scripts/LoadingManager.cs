using UnityEngine;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    private static int levelToLoad = 0;

    private static LoadingManager managerSingleton = null;

    private void Awake()
    {
        if(managerSingleton == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadLevel(int index)
    {
        levelToLoad = index;
        Application.LoadLevel(1);
    }

    private IEnumerator OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            yield return null;  // Skip a frame so the Loading screen draws
            if (Application.HasProLicense())
            {
               Application.LoadLevelAsync(levelToLoad);
               yield break;
            }
            else
            {
                Application.LoadLevel(levelToLoad);
                yield break;
            }
        }

        if(level == levelToLoad)    // reset to default once the target level loads
        {
            levelToLoad = 0;
        }
    }
}
