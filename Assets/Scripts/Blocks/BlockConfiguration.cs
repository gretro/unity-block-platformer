using System;
using UnityEngine.Tilemaps;

public enum BlockStatus
{
    Pristine,
    Damaged
}

[Serializable]
public class BlockConfiguration
{
    public BlockSource source;
    public Tile tile;
    public Tile damagedTile;

    public BlockStatus? GetBlockStatus(TileBase currentTile)
    {
        var isPristing = currentTile == tile;
        var isDamaged = currentTile == damagedTile;

        if (isPristing || isDamaged)
        {
            var status = isPristing ? BlockStatus.Pristine : BlockStatus.Damaged;
            return status;
        }

        return null;
    }
}