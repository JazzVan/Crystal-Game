using UnityEngine;
using UnityEngine.Tilemaps;

public class GemPlacer : MonoBehaviour
{
    public Tilemap[] tilemaps;
    public TileBase gemTile;

    void Start()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            PlaceGem(tilemap);
        }
    }

    void PlaceGem(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        // Collect all valid dirt cells
        var validCells = new System.Collections.Generic.List<Vector3Int>();

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                validCells.Add(pos);
            }
        }

        if (validCells.Count == 0)
            return;

        // Pick one random cell
        Vector3Int chosenCell =
            validCells[Random.Range(0, validCells.Count)];

        tilemap.SetTile(chosenCell, gemTile);
    }
}
