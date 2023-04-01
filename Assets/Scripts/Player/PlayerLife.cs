using Assets.Scripts.Utils;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [Header("Collisions")]
    [SerializeField]
    private LayerMask enemyLayer;

    [Header("Channels")]
    [SerializeField]
    private LevelChannel levelChannel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ColliderUtils.IsOnLayerMask(collision.gameObject, enemyLayer))
        {
            levelChannel.PlayerDied.RaiseEvent();
        }
    }
}
