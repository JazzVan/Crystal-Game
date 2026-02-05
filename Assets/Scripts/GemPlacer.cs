using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GemPlacer : MonoBehaviour
{
    public Tilemap[] tilemaps;

    [Header("Gem Definitions")]
    public GemData[] gemDefinitions;

    [Header("Spawn Settings")]
    [Range(0, 10)]
    public int minGemsPerLayer = 0;

    [Range(1, 5)]
    public int maxGemsPerLayer = 3;

    void Start()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            PlaceGemsOnTilemap(tilemap);
        }
    }

    GemData GetRandomGemByWeight()
    {
        int totalWeight = 0;

        foreach (var gem in gemDefinitions)
            totalWeight += gem.weight;

        int roll = Random.Range(0, totalWeight);

        foreach (var gem in gemDefinitions)
        {
            roll -= gem.weight;
            if (roll < 0)
                return gem;
        }

        // Fallback (should never happen)
        return gemDefinitions[0];
    }


    void PlaceGemsOnTilemap(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        List<Vector3Int> validCells = new List<Vector3Int>();

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                validCells.Add(pos);
            }
        }

        if (validCells.Count == 0)
            return;

        int gemsToPlace = Random.Range(minGemsPerLayer, maxGemsPerLayer + 1);

        gemsToPlace = Mathf.Min(gemsToPlace, validCells.Count);

        for (int i = 0; i < gemsToPlace; i++)
        {
            // Pick a random cell (no duplicates)
            int cellIndex = Random.Range(0, validCells.Count);
            Vector3Int cell = validCells[cellIndex];
            validCells.RemoveAt(cellIndex);

            // Pick a random gem type
            GemData gem = GetRandomGemByWeight();


            tilemap.SetTile(cell, gem.tile);
        }
    }
}
