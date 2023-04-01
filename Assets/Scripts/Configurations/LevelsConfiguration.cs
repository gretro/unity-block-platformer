using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelsConfiguration", menuName = "BlockPlatformer/Configuration/LevelsConfiguration")]
public class LevelsConfiguration : ScriptableObject
{
    [Serializable]
    public class Level
    {
        [SerializeField]
        public string levelName;

        [SerializeField]
        public string sceneName;
    }

    [SerializeField]
    public List<Level> levels = new();

    [SerializeField]
    public int startLevelIndex = 0;

    [NonSerialized]
    private int currentLevelIndex = -1;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (Application.isEditor)
        {
            // This is simpler for right now
            RestartLevel();
        } else
        {
            currentLevelIndex++;

            var levelToLoad = levels.ElementAtOrDefault(currentLevelIndex);
            if (levelToLoad == null)
            {
                // TODO: Review this. We should probably go to a High Score scene, then the main menu.
                levelToLoad = levels[0];
            }

            SceneManager.LoadScene(levelToLoad.sceneName);
        }
    }
}
