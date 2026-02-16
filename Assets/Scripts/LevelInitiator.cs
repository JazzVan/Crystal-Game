using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class LevelInitiator : MonoBehaviour
{
    public Tilemap[] tilemaps;

    [Header("Gem Definitions")]
    public GemData[] gemDefinitions;

    [Header("Spawn Settings")]
    [Range(0, 10)]
    public int minGemsPerLayer = 0;

    [Range(1, 5)]
    public int maxGemsPerLayer = 3;


    [Header("Fracture Settings")]
    public HazzardData fractureTile;

    [Range(0, 5)]
    public int minFracturesPerLayer = 1;

    [Range(0, 5)]
    public int maxFracturesPerLayer = 2;

    [Header("Pressure Point Settings")]
    public HazzardData pressurePointTile;

    [Range(0, 5)]
    public int minPressurePerLayer = 0;

    [Range(0, 5)]
    public int maxPressurePerLayer = 1;

    public static LevelInitiator Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            SpawnGemsAndHazzards(tilemap);
        }
    }

    public void RegenerateLevel()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            tilemap.ClearAllTiles();

            // If you originally had a base sediment tile,
            // you must refill it here before spawning gems/hazards

            SpawnGemsAndHazzards(tilemap);
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


    void SpawnGemsAndHazzards(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;

        List<Vector3Int> validCells = new List<Vector3Int>();

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
                validCells.Add(pos);
        }

        if (validCells.Count == 0)
            return;

        // --------- PLACE GEMS ---------
        int gemsToPlace = Random.Range(minGemsPerLayer, maxGemsPerLayer + 1);
        gemsToPlace = Mathf.Min(gemsToPlace, validCells.Count);

        for (int i = 0; i < gemsToPlace; i++)
        {
            int index = Random.Range(0, validCells.Count);
            Vector3Int cell = validCells[index];
            validCells.RemoveAt(index);

            GemData gem = GetRandomGemByWeight();
            tilemap.SetTile(cell, gem.tile);
        }

        // --------- PLACE FRACTURES ---------
        int minFrac = Mathf.Min(minFracturesPerLayer, maxFracturesPerLayer);
        int maxFrac = Mathf.Max(minFracturesPerLayer, maxFracturesPerLayer);

        int fracturesToPlace = Random.Range(minFrac, maxFrac + 1);
        fracturesToPlace = Mathf.Min(fracturesToPlace, validCells.Count);

        for (int i = 0; i < fracturesToPlace; i++)
        {
            int index = Random.Range(0, validCells.Count);
            Vector3Int cell = validCells[index];
            validCells.RemoveAt(index);

            tilemap.SetTile(cell, fractureTile);
        }

        // --------- PLACE PRESSURE POINTS ---------
        int minPress = Mathf.Min(minPressurePerLayer, maxPressurePerLayer);
        int maxPress = Mathf.Max(minPressurePerLayer, maxPressurePerLayer);

        int pressureToPlace = Random.Range(minPress, maxPress + 1);
        pressureToPlace = Mathf.Min(pressureToPlace, validCells.Count);

        for (int i = 0; i < pressureToPlace; i++)
        {
            int index = Random.Range(0, validCells.Count);
            Vector3Int cell = validCells[index];
            validCells.RemoveAt(index);

            tilemap.SetTile(cell, pressurePointTile);
        }


    }

}
