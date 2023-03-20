using System;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public MovementController entity;
    public BlockCreator blockCreator;

    public Collider2D cliffTrigger;
    public Collider2D forwardTrigger;

    public bool smashDestructibleBlock;
    public bool canFall;
    public bool debug;

    private float horizontalMovement = 1f;
    private int blocksLayer;

    private void Start()
    {
        blocksLayer = LayerMask.GetMask("Blocks");
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

                horizontalMovement *= -1;
            }
        }

        var blockAhead = forwardTrigger.IsTouchingLayers(blocksLayer);
        if (blockAhead)
        {
            var blockDestroyed = false;

            if (smashDestructibleBlock)
            {
                blockDestroyed = blockCreator.DestroyBlock(entity.transform.position, (int)horizontalMovement);
            }

            if (!smashDestructibleBlock || !blockDestroyed)
            {
                if (debug)
                {
                    Debug.Log("Enemy changing direction because of a block");
                }
                
                horizontalMovement *= -1;
            }
        }

        entity.Move(horizontalMovement);
    }
}
