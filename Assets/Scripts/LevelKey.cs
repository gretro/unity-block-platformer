using UnityEngine;

public class LevelKey : MonoBehaviour
{
    public LevelExit levelExit;

    private int playerLayer;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            levelExit.OpenExit();
            Destroy(gameObject);
        }
    }
}
