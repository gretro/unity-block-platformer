using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

[RequireComponent(typeof(Tilemap))]
public class BlockManager : MonoBehaviour
{
    [Header("Block configurations")]
    [SerializeField]
    private List<BlockConfiguration> blockConfigurations;

    [Header("Level area")]
    [SerializeField] 
    private int gridWidth;
    [SerializeField] 
    private int gridHeight;

    [Header("Collisions")]
    [SerializeField]
    private LayerMask noBlockLayers;

    [Header("Channels")]
    [SerializeField]
    private BlockChannel blockChannel = default;

    [Header("Debug")]
    [SerializeField]
    private bool debug;

    private Dictionary<BlockSource, BlockConfiguration> _configBySource;
    private Dictionary<TileBase, BlockConfiguration> _configByTile = new();
    private Tilemap _tilemap;
    private int _heightLimit;
    private int _widthLimit;

    private void Awake()
    {
        _configBySource = blockConfigurations.ToDictionary(i => i.source);
        blockConfigurations.ForEach(config => {
            _configByTile.Add(config.tile, config);
            _configByTile.Add(config.damagedTile, config);
        });

        _heightLimit = gridHeight / 2;
        _widthLimit = gridWidth / 2;
    }

    private void OnEnable()
    {
        blockChannel.CastOrBanishBlock.EventHandler += CastOrBanishBlock;
        blockChannel.DamageBlock.EventHandler += DamageBlock;
        blockChannel.DestroyBlock.EventHandler += DestroyBlock;
        blockChannel.BlockDestructibleQuery.QueryHandler = IsBlockDestructible;
    }

    private void OnDisable()
    {
        blockChannel.CastOrBanishBlock.EventHandler -= CastOrBanishBlock;
        blockChannel.DamageBlock.EventHandler -= DamageBlock;
        blockChannel.DestroyBlock.EventHandler -= DestroyBlock;
        blockChannel.BlockDestructibleQuery.QueryHandler = default;
    }

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    public void CastOrBanishBlock(Vector3 worldPosition, Vector3Int offset, BlockSource source)
    {
        var gridPosition = GetGridPosition(worldPosition, offset);
        if (!gridPosition.HasValue)
        {
            return;
        }

        var tileAtPosition = _tilemap.GetTile(gridPosition.Value);
        // No tile found at the requested position. Attempting to cast a block.
        if (tileAtPosition == null)
        {
            var castPosition = GetWorldPosition(gridPosition.Value);
            var maybeCollider = Physics2D.OverlapBox(castPosition, Vector2.one, 0f, noBlockLayers);
            if (maybeCollider != null)
            {
                if(debug)
                {
                    Debug.Log($"Cannot cast block at ({gridPosition.Value.x}, {gridPosition.Value.y}) because it overlaps {maybeCollider.gameObject.name}");
                }

                return;
            }

            var blockConfig = _configBySource.GetValueOrDefault(source);
            if (blockConfig == null)
            {
                Debug.LogError($"No block configuration found for source {source}");
                return;
            }

            _tilemap.SetTile(gridPosition.Value, blockConfig.tile);
            return;
        }
        else
        {
            var blockConfig = _configByTile.GetValueOrDefault(tileAtPosition);

            // Found a block at given position, but block is indestructible (or not configured)
            if (blockConfig == null)
            {
                if (debug)
                {
                    Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is indestructible (or not configured)");
                }

                return;
            }
            if (debug)
            {
                Debug.Log($"Banishing block at ({gridPosition.Value.x}, {gridPosition.Value.y})");
            }

            _tilemap.SetTile(gridPosition.Value, null);
            return;
        }
    }

    public void DestroyBlock(Vector3 worldPosition, Vector3Int offset)
    {
        var gridPosition = GetGridPosition(worldPosition, offset);
        if (!gridPosition.HasValue)
        {
            return;
        }

        var tileAtPosition = _tilemap.GetTile(gridPosition.Value);

        // No tile found at requested position. Cannot damage it
        if (tileAtPosition == null)
        {
            if (debug)
            {
                Debug.Log($"No block at ({gridPosition.Value.x}, {gridPosition.Value.y}). Cannot destroy.");
            }

            return;
        }

        var blockConfig = _configByTile.GetValueOrDefault(tileAtPosition);
        // Found a block at given position, but block is indestructible (or not configured)
        if (blockConfig == null)
        {
            if (debug)
            {
                Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is indestructible (or not configured)");
            }

            return;
        }

        if (debug)
        {
            Debug.Log($"Destroying block at ({gridPosition.Value.x}, {gridPosition.Value.y}).");
        }
        _tilemap.SetTile(gridPosition.Value, null);
    }

    public void DamageBlock(Vector3 worldPosition, Vector3Int offset)
    {
        var gridPosition = GetGridPosition(worldPosition, offset);
        if (!gridPosition.HasValue)
        {
            return;
        }

        var tileAtPosition = _tilemap.GetTile(gridPosition.Value);

        // No tile found at requested position. Cannot damage it
        if (tileAtPosition == null)
        {
            if (debug)
            {
                Debug.Log($"No block at ({gridPosition.Value.x}, {gridPosition.Value.y}). Cannot damage.");
            }

            return;
        }

        var blockConfig = _configByTile.GetValueOrDefault(tileAtPosition);
        // Found a block at given position, but block is indestructible (or not configured)
        if (blockConfig == null)
        {
            if (debug)
            {
                Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is indestructible (or not configured)");
            }

            return;
        }

        if (tileAtPosition == blockConfig.damagedTile)
        {
            if (debug)
            {
                Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is already damaged. Destroying it.");
            }

            _tilemap.SetTile(gridPosition.Value, null);
        } else
        {
            if (debug)
            {
                Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is pristine. Damaging it.");
            }

            _tilemap.SetTile(gridPosition.Value, blockConfig.damagedTile);
        }
    }

    public bool IsBlockDestructible(Vector3 worldPosition, Vector3Int offset)
    {
        var gridPosition = GetGridPosition(worldPosition, offset);
        if (!gridPosition.HasValue)
        {
            return false;
        }

        var tileAtPosition = _tilemap.GetTile(gridPosition.Value);
        if (tileAtPosition == null)
        {
            return false;
        }

        var blockConfig = _configByTile.GetValueOrDefault(tileAtPosition);
        // Found a block at given position, but block is indestructible (or not configured)
        if (blockConfig == null)
        {
            if (debug)
            {
                Debug.Log($"Block at ({gridPosition.Value.x}, {gridPosition.Value.y}) is indestructible (or not configured)");
            }

            return false;
        }

        return true;
    }

    private Vector3Int? GetGridPosition(Vector3 worldPosition, Vector3Int offset)
    {
        var cellPosition = _tilemap.WorldToCell(worldPosition);
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

    private Vector2 GetWorldPosition(Vector3Int gridPosition)
    {
        var worldPos = _tilemap.CellToWorld(gridPosition) + _tilemap.tileAnchor;

        Debug.Log($"World position: ({worldPos.x}, {worldPos.y})");
        return worldPos;
    }
}
