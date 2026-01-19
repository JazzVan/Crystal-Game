using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TilePlusRemover : ToolWithDurability
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
            Vector3Int clickedCell = Vector3Int.zero;

            // 1️⃣ Define the center + 4 directions
            Vector3[] directions = new Vector3[]
            {
                Vector3.zero,                    // center
                Vector3.right * 1f,              // right
                Vector3.left * 1f,               // left
                Vector3.up * 1f,                 // up
                Vector3.down * 1f                // down
            };

            foreach (var offset in directions)
            {
                Vector2 checkPos = mouseWorldPos + (Vector2)offset;

                // 2️⃣ Raycast at this position to hit all colliders
                RaycastHit2D[] hits = Physics2D.RaycastAll(checkPos, Vector2.zero);

                if (hits.Length == 0)
                    continue;

                // 3️⃣ Sort hits by topmost renderer (highest sortingOrder)
                var topHit = hits
                    .Select(h => new
                    {
                        Hit = h,
                        Renderer = h.collider.GetComponent<TilemapRenderer>()
                    })
                    .Where(x => x.Renderer != null)
                    .OrderByDescending(x => x.Renderer.sortingOrder)
                    .FirstOrDefault();

                if (topHit == null)
                    continue;

                Tilemap tilemap = topHit.Hit.collider.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    Vector3Int cellPos = tilemap.WorldToCell(checkPos);
                    if (tilemap.HasTile(cellPos))
                    {
                        tilemap.SetTile(cellPos, null);
                    }
                }
            }

            ConsumeUse();
        }
    }
}
