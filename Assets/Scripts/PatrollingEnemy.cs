using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PatrollingEnemy : MonoBehaviour
{
    [Header("Collisions")]
    [SerializeField]
    private Collider2D cliffTrigger;
    [SerializeField]
    public Collider2D forwardTrigger;
    [SerializeField]
    private LayerMask blocksLayer;

    [Header("Channels")]
    [SerializeField]
    private BlockChannel blockChannel = default;

    [Header("Behaviours")]
    [SerializeField]
    private bool smashDestructibleBlock;
    [SerializeField]
    private bool canFall;

    [Header("Debug")]
    [SerializeField]
    private bool debug;

    private MovementController _entity;
    private float _horizontalMovement = 1f;

    private void Start()
    {
        _entity = GetComponent<MovementController>();
    }

    private void Update()
    {
        if (!canFall)
        {
            var hasGroundAhead = cliffTrigger.IsTouchingLayers(blocksLayer);
            if (!hasGroundAhead)
            {
                if (debug)
                {
                    Debug.Log("Enemy changing direction because of a cliff");
                }

                _horizontalMovement *= -1;
            }
        }

        var blockAhead = forwardTrigger.IsTouchingLayers(blocksLayer);
        if (blockAhead)
        {
            var isBlockDestructible = false;

            if (smashDestructibleBlock)
            {
                var offset = Vector3Int.right * (int)_horizontalMovement;

                isBlockDestructible = blockChannel.BlockDestructibleQuery.RaiseQuery(_entity.transform.position, offset);
                if (isBlockDestructible)
                {
                    blockChannel.DestroyBlock.RaiseEvent(_entity.transform.position, offset);
                }
            }

            if (!smashDestructibleBlock || !isBlockDestructible)
            {
                if (debug)
                {
                    Debug.Log("Enemy changing direction because of a block");
                }
                
                _horizontalMovement *= -1;
            }
        }

        _entity.Move(_horizontalMovement);
    }
}
