using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
#pragma warning disable IDE0051 // Remove unused private members
    private readonly int LevelCount = 2;
#pragma warning restore IDE0051 // Remove unused private members

    public void OnGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnLevelCompleted()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
#else
        var sceneIx = SceneManager.GetActiveScene().buildIndex;
        if (sceneIx < LevelCount - 1) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            // Restart the game
            SceneManager.LoadScene(0);
        }
#endif
    }
}
