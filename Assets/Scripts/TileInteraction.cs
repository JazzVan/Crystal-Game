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
            case ToolType.PrecisionMiningLaser:
                removedSomething = RemoveTopTile(mouseWorldPos);
                break;

            case ToolType.Hammer:
                removedSomething = RemovePlus(mouseWorldPos);
                break;

            case ToolType.Shovel:
                removedSomething = Remove2x2Top2(mouseWorldPos);
                break;

            case ToolType.HighPressureHose:
                removedSomething = Remove2Down(mouseWorldPos);
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

    bool RemovePlus(Vector2 center)
    {
        Vector2[] offsets =
        {
            Vector2.zero,
            Vector2.right,
            Vector2.left,
            Vector2.up,
            Vector2.down
        };

        bool removed = false;

        foreach (var offset in offsets)
        {
            removed |= RemoveTopTile(center + offset);
        }

        return removed;
    }

    bool Remove2x2Top2(Vector2 center)
    {
        Vector2[] offsets =
        {
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 1), new Vector2(1, 1)
        };

        bool removed = false;

        foreach (var offset in offsets)
        {
            removed |= Remove2Down(center + offset);
        }

        return removed;
    }

    void RemoveTile(Tilemap tilemap, Vector2 worldPos)
    {
        Vector3Int cell = tilemap.WorldToCell(worldPos);
        if (tilemap.HasTile(cell))
            tilemap.SetTile(cell, null);
    }
}
