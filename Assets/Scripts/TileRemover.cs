using UnityEngine;
using UnityEngine.Tilemaps;

public class TileRemover : ToolWithDurability
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

            // Raycast against all 2D colliders at this point
            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero);

            if (hits.Length > 0)
            {
                Tilemap topTilemap = null;
                int highestOrder = int.MinValue;

                foreach (RaycastHit2D hit in hits)
                {
                    Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
                    if (tilemap != null)
                    {
                        var renderer = tilemap.GetComponent<TilemapRenderer>();
                        if (renderer != null && renderer.sortingOrder > highestOrder)
                        {
                            highestOrder = renderer.sortingOrder;
                            topTilemap = tilemap;
                        }
                    }
                }

                // Remove tile only from the top-most tilemap
                if (topTilemap != null)
                {
                    Vector3Int cellPos = topTilemap.WorldToCell(mouseWorldPos);
                    if (topTilemap.HasTile(cellPos))
                    {
                        topTilemap.SetTile(cellPos, null);
                    }
                }
            }

            ConsumeUse();
        }
    }
}

