using UnityEngine;

public class FullFloorGenerationWidget : FloorGenerationWidget
{
    public override bool? FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        return true;
    }
}