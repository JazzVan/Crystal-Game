using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Gems/Gem Data")]
public class GemData : ScriptableObject
{
    public GemType gemType;
    public TileBase tile;

    [Range(1, 100)]
    public int weight = 10;

}
