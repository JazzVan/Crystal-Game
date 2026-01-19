using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Tile2DownRemover : ToolWithDurability
{
    private Camera mainCamera;

    void Start()
    {
        base.Start();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBroken)
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Raycast all Tilemap colliders at the clicked point
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero);

            if (hits.Length == 0)
                return;

            // Sort hits from top (highest sortingOrder) to bottom
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

            // 1️⃣ Remove the topmost tile
            var top = orderedHits[0];
            Vector3Int topCell = top.Tilemap.WorldToCell(mouseWorldPos);
            top.Tilemap.SetTile(topCell, null);

            // 2️⃣ Remove the tile directly under it (if there is one)
            if (orderedHits.Count > 1)
            {
                var under = orderedHits[1];
                Vector3Int underCell = under.Tilemap.WorldToCell(mouseWorldPos);
                under.Tilemap.SetTile(underCell, null);
            }

            ConsumeUse();
        }
    }
}
