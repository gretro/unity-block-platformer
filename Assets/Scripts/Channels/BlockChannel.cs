using Assets.Scripts.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockChannel", menuName = "BlockPlatformer/Channels/BlockChannel")]
public class BlockChannel : ScriptableObject
{
    // Events
    public readonly GameEvent<Vector3, Vector3Int, BlockSource> CastOrBanishBlock = new("CastOrBanishBlock");
    public readonly GameEvent<Vector3, Vector3Int> DamageBlock = new("DamageBlock");
    public readonly GameEvent<Vector3, Vector3Int> DestroyBlock = new("DestroyBlock");

    // Queries
    public readonly GameQuery<Vector3, Vector3Int, bool> BlockDestructibleQuery = new("BlockDestructible");
}
