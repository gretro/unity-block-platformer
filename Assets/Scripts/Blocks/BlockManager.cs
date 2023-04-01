using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private Tile indestructibleTile;

    [SerializeField]
    private List<BlockConfiguration> blockConfigurations;

    [SerializeField] 
    private int gridWidth;
    [SerializeField] 
    private int gridHeight;

    [SerializeField]
    private bool debug;

    private Dictionary<BlockSource, BlockConfiguration> _configurations;
    private Tilemap _tilemap;
    private int _heightLimit;
    private int _widthLimit;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _configurations = blockConfigurations.ToDictionary(i => i.source);
        _heightLimit = gridHeight / 2;
        _widthLimit = gridWidth / 2;
    }

    public bool CreateOrBanishBlock(Vector3 worldPosition, Vector3Int offset, BlockSource source)
    {
        var gridPosition = GetGridPosition(worldPosition, offset);
        if (!gridPosition.HasValue)
        {
            return false;
        }

        var tileAtPosition = _tilemap.GetTile(gridPosition.Value);
        if (tileAtPosition == null)
        {
            var blockConfig = _configurations.GetValueOrDefault(source);
            if (blockConfig == null)
            {
                Debug.LogError($"No block configuration found for source {source}");
                return false;
            }

            _tilemap.SetTile(gridPosition.Value, blockConfig.tile);
            return true;
        } 
        else if (tileAtPosition == indestructibleTile)
        {
            if (debug)
            {
                Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is indestructible");
            }

            return false;
        }
        else
        {
            if (debug)
            {
                Debug.Log($"Banishing block at ({gridPosition.Value.x}, {gridPosition.Value.y})");
            }

            _tilemap.SetTile(gridPosition.Value, null);
            return true;
        }
    }

    public bool DestroyBlock(Vector3 worldPosition, Vector3Int offset)
    {
        return false;
    }

    private bool DamageBlock(Vector3Int gridPosition)
    {
        return false;
    }

    private BlockConfiguration GetBlockConfigurationForTile(TileBase tile)
    {
        var result = blockConfigurations.FirstOrDefault(blockConfig => blockConfig.GetBlockStatus(tile).HasValue);
        return result;
    }

    private Vector3Int? GetGridPosition(Vector3 worldPosition, Vector3Int offset)
    {
        var cellPosition = _tilemap.WorldToCell(worldPosition + _tilemap.tileAnchor);
        var offsetCellPosition = cellPosition + offset;

        if (debug)
        {
            Debug.Log($"Cell position: ({offsetCellPosition.x}, {offsetCellPosition.y})");
        }

        if (Math.Abs(offsetCellPosition.x) > _widthLimit || Math.Abs(offsetCellPosition.y) > _heightLimit)
        {
            if (debug)
            {
                Debug.Log("Cell is out of position");
            }

            return null;
        }

        return offsetCellPosition;
    }
}
