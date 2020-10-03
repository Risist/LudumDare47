using UnityEngine;

public class CrossLineWidget : FloorGenerationWidget
{
    public int _lineWidth;
    public FloorGenerationOutcome Outcome;

    public override FloorGenerationOutcome FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        if (Mathf.Abs(queriedPosition.x) < _lineWidth || Mathf.Abs(queriedPosition.y) < _lineWidth)
        {
            return Outcome;
        }

        return FloorGenerationOutcome.None;
    }
}