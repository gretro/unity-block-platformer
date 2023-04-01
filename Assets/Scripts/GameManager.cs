using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    private LevelsConfiguration levelsConfig;

    [Header("Channels")]
    [SerializeField]
    private LevelChannel levelChannel;

    private void OnEnable()
    {
        levelChannel.PlayerDied.EventHandler += OnPlayerDied;
        levelChannel.LevelExit.EventHandler += OnLevelCompleted;
    }

    private void OnDisable()
    {
        levelChannel.PlayerDied.EventHandler -= OnPlayerDied;
        levelChannel.LevelExit.EventHandler -= OnLevelCompleted;
    }

    public void OnPlayerDied()
    {
        levelsConfig.RestartLevel();
    }

    public void OnLevelCompleted()
    {
        levelsConfig.NextLevel();
    }
}
