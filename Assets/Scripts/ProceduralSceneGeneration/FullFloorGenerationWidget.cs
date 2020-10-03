using UnityEngine;

public class FullFloorGenerationWidget : FloorGenerationWidget
{
    public override FloorGenerationOutcome FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        return FloorGenerationOutcome.On;
    }
}