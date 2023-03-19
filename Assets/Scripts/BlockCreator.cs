using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockCreator : MonoBehaviour
{
    public Tilemap indestructibleTileMap;
    public Tilemap destructableTileMap;
    public Tile blockTile;

    public Transform creator;
    public GameObject cursorPrefab;

    public bool debug = false;

    private IDirectional creatorDirection;
    private Transform cursor;

    private void Start()
    {
        creatorDirection = creator.GetComponent<IDirectional>();
        if (creatorDirection == null)
        {
            Debug.LogWarning($"Creator is not IDirectional");
        }

        if (cursorPrefab != null)
        {
            cursor = Instantiate(cursorPrefab).transform;
            UpdateCursor();
        }
    }

    private void FixedUpdate()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (cursor == null)
        {
            return;
        }

        var cursorGridPosition = GetCursorGridPosition();
        var worldPos = destructableTileMap.CellToLocal(cursorGridPosition) + destructableTileMap.tileAnchor;

        cursor.position = worldPos;
    }

    public void CreateOrDestroyBlock()
    {
        var gridPosition = GetCursorGridPosition();
        CreateOrDestroyBlockAtGridPosition(gridPosition, false);
    }

    public void CreateOrDestroyGroundBlock()
    {
        var gridPosition = GetCursorGridPosition() + Vector3Int.down;
        CreateOrDestroyBlockAtGridPosition(gridPosition, false);
    }

    private void CreateOrDestroyBlockAtGridPosition(Vector3Int gridPosition, bool forceDestroy)
    {
        if (debug)
        {
            Debug.Log($"Creating or destroying block at {gridPosition}. Force Destroy: {forceDestroy}");
        }

        if (indestructibleTileMap.HasTile(gridPosition))
        {
            if (debug)
            {
                Debug.Log("Indestructible blocks cannot be destroyed");
            }

            return;
        }

        if (destructableTileMap.HasTile(gridPosition) || forceDestroy)
        {
            if (debug)
            {
                Debug.Log($"Destroying block");
            }

            destructableTileMap.SetTile(gridPosition, null);
        }
        else
        {
            Debug.Log($"Creating block: {blockTile.name}");

            destructableTileMap.SetTile(gridPosition, blockTile);
        }
    }

    public void DestroyAboveBlock(Vector3 position)
    {
        var blockPos = destructableTileMap.WorldToCell(position) + Vector3Int.up;
        CreateOrDestroyBlockAtGridPosition(blockPos, true);
    }

    private Vector3Int GetCursorGridPosition()
    {
        var direction = creatorDirection?.Direction ?? 1;
        var cellPosition = destructableTileMap.WorldToCell(creator.position + Vector3.right * direction);

        return cellPosition;
    }
}
