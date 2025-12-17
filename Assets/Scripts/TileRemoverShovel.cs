using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileRemoverShovel : ToolWithDurability
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBroken)
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Mine2x2x2(mouseWorldPos);

            ConsumeUse();
        }
    }

    void Mine2x2x2(Vector2 center)
    {
        // Offsets for a 2×2 area (center + [0 or 1] in each axis)
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 1), new Vector2(1, 1)
        };

        foreach (var offset in offsets)
        {
            Vector2 pos = center + offset;
            RemoveTop2Layers(pos);
        }
    }

    void RemoveTop2Layers(Vector2 point)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(point, Vector2.zero);

        if (hits.Length == 0)
            return;

        // Sort from highest sorting layer to lowest
        var orderedHits = hits
            .Select(h => new
            {
                Hit = h,
                Renderer = h.collider.GetComponent<TilemapRenderer>(),
                Tilemap = h.collider.GetComponent<Tilemap>()
            })
            .Where(x => x.Renderer != null && x.Tilemap != null)
            .OrderByDescending(x => x.Renderer.sortingOrder)
            .ToList();

        if (orderedHits.Count == 0)
            return;

        // Remove top
        RemoveTileAtPoint(orderedHits[0].Tilemap, point);

        // Remove next layer if exists
        if (orderedHits.Count > 1)
            RemoveTileAtPoint(orderedHits[1].Tilemap, point);
    }

    void RemoveTileAtPoint(Tilemap tilemap, Vector2 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        if (tilemap.HasTile(cellPos))
            tilemap.SetTile(cellPos, null);
    }
}