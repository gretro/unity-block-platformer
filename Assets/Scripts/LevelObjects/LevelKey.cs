using Assets.Scripts.Utils;
using UnityEngine;

public class LevelKey : MonoBehaviour
{
    [Header("Collisions")]
    [SerializeField]
    private LayerMask playerLayer;

    [Header("Channels")]
    [SerializeField]
    private LevelChannel levelChannel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ColliderUtils.IsOnLayerMask(collision.gameObject, playerLayer))
        {
            levelChannel.ExitOpen.RaiseEvent();
            Destroy(gameObject);
        }
    }
}
