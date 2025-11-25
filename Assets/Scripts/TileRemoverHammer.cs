using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileRemoverHammer : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Mine3x3x3(mouseWorldPos);
        }
    }

    void Mine3x3x3(Vector2 center)
    {
        // Offsets for a 3×3 area
        Vector2[] offsets = new Vector2[]
        {
            new Vector2(-1,  1), new Vector2(0,  1), new Vector2(1,  1),
            new Vector2(-1,  0), new Vector2(0,  0), new Vector2(1,  0),
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1)
        };

        foreach (var offset in offsets)
        {
            Vector2 pos = center + offset;
            RemoveTop3Layers(pos);
        }
    }

    void RemoveTop3Layers(Vector2 point)
    {
        // Raycast all tilemap colliders at the point
        RaycastHit2D[] hits = Physics2D.RaycastAll(point, Vector2.zero);

        if (hits.Length == 0)
            return;

        // Sort hits top → bottom (by sortingOrder)
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

        // Remove top layer (1st tile)
        RemoveTileAtPoint(orderedHits[0].Tilemap, point);

        // Remove second layer (2nd tile) if it exists
        if (orderedHits.Count > 1)
            RemoveTileAtPoint(orderedHits[1].Tilemap, point);

        // Remove third layer (3rd tile) if it exists
        if (orderedHits.Count > 2)
            RemoveTileAtPoint(orderedHits[2].Tilemap, point);
    }

    void RemoveTileAtPoint(Tilemap tilemap, Vector2 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        if (tilemap.HasTile(cellPos))
            tilemap.SetTile(cellPos, null);
    }
}