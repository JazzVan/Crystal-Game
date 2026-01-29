using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;


class TileHit
{
    public RaycastHit2D Hit;
    public TilemapRenderer Renderer;
    public Tilemap Tilemap;
}

public class TileInteraction : MonoBehaviour
{
    private Camera cam;
    public TileBase[] gemTiles;



    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (!ToolManager.Instance.CanUseActiveTool())
            return;

        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        bool removedSomething = false;

        switch (ToolManager.Instance.activeTool)
        {
            case ToolType.Pickaxe:
                removedSomething = RemoveTopTile(mouseWorldPos);
                break;

            case ToolType.Hammer:
                removedSomething = RemoveHammer(mouseWorldPos);
                break;

            case ToolType.Shovel:
                removedSomething = RemoveShovel(mouseWorldPos);
                break;

            case ToolType.HighPressureHose:
                removedSomething = Remove2Down(mouseWorldPos);
                break;

            case ToolType.PrecisionMiningLaser:
                removedSomething = RemoveLaser(mouseWorldPos);
                break;
        }

        if (removedSomething)
        {
            ToolManager.Instance.ConsumeActiveToolUse();
        }
    }


    // ---------------- RAYCAST HELPERS ----------------

    RaycastHit2D[] Raycast(Vector2 pos)
    {
        return Physics2D.RaycastAll(pos, Vector2.zero);
    }

    List<TileHit> GetOrderedHits(Vector2 pos)
    {
        return Raycast(pos)
            .Select(h => new TileHit
            {
                Hit = h,
                Renderer = h.collider.GetComponent<TilemapRenderer>(),
                Tilemap = h.collider.GetComponent<Tilemap>()
            })
            .Where(x => x.Renderer != null && x.Tilemap != null)
            .OrderByDescending(x => x.Renderer.sortingOrder)
            .ToList();
    }


    bool RemoveTopTile(Vector2 pos)
    {
        var hits = GetOrderedHits(pos);
        if (hits.Count == 0)
            return false;

        RemoveTile(hits[0].Tilemap, pos);
        return true;
    }

    bool Remove2Down(Vector2 pos)
    {
        var hits = GetOrderedHits(pos);
        if (hits.Count == 0)
            return false;

        RemoveTile(hits[0].Tilemap, pos);

        if (hits.Count > 1)
            RemoveTile(hits[1].Tilemap, pos);

        return true;
    }

    bool RemoveTopNLayers(Vector2 pos, int layers)
    {
        var hits = GetOrderedHits(pos);
        if (hits.Count == 0)
            return false;

        bool removed = false;

        for (int i = 0; i < layers && i < hits.Count; i++)
        {
            RemoveTile(hits[i].Tilemap, pos);
            removed = true;
        }

        return removed;
    }


    bool RemoveHammer(Vector2 center)
    {
        bool removed = false;

        int r = Random.Range(3, 6); // 3, 4, or 5 (int)

        // 3x3 area centered on click
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 pos = center + new Vector2(x, y);
                removed |= RemoveTopNLayers(pos, r);
            }
        }

        return removed;
    }


    bool RemoveShovel(Vector2 center)
    {
        bool removed = false;

        // Random layers: 2 to 4 (inclusive)
        int layers = Random.Range(2, 5);

        Vector2[] offsets =
        {
        new Vector2(0, 0), new Vector2(1, 0),
        new Vector2(0, 1), new Vector2(1, 1)
        };

        foreach (var offset in offsets)
        {
            removed |= RemoveTopNLayers(center + offset, layers);
        }

        return removed;
    }

    bool RemoveLaser(Vector2 pos)
    {
        var hits = GetOrderedHits(pos);
        if (hits.Count == 0)
            return false;

        bool removedSomething = false;

        foreach (var hit in hits)
        {
            Tilemap tilemap = hit.Tilemap;
            Vector3Int cell = tilemap.WorldToCell(pos);

            TileBase tile = tilemap.GetTile(cell);
            if (tile == null)
                continue;

            // Remove the tile
            removedSomething = true;

            // If it's a gem, remove it and STOP
            if (gemTiles.Contains(tile))
            {
                GemCounter.Instance.AddGem(1);
                tilemap.SetTile(cell, null);
                break;
            }

            tilemap.SetTile(cell, null);
        }

        return removedSomething;
    }



    void RemoveTile(Tilemap tilemap, Vector2 worldPos)
    {
        Vector3Int cell = tilemap.WorldToCell(worldPos);

        TileBase tile = tilemap.GetTile(cell);
        Debug.Log($"Removed tile: {tile.name}");

        if (tile == null)
            return;

        if (gemTiles.Contains(tile))
        {
            GemCounter.Instance.AddGem(1);
        }


        tilemap.SetTile(cell, null);
    }

}
