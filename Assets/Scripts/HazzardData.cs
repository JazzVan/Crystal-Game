using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Hazzard Tile")]
public class HazzardData : Tile
{
    public HazzardType type;

    [Header("Cave In Effect")]
    public int caveInDamage = 1;

    [Header("Behavior")]
    public bool destroyOnHit = true;
}
