using UnityEngine;

public class SquareRingWidget : FloorGenerationWidget
{
    public float _radiusMin;
    public float _radiusMax;
    public FloorGenerationOutcome Outcome;

    public override FloorGenerationOutcome FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        var radiuses = new Vector2(2f*queriedPosition.x / roomSize.x, 2f*queriedPosition.y /roomSize.y);

        var radiusFactor = Mathf.Max(Mathf.Abs(radiuses.x), Mathf.Abs(radiuses.y));

        if (radiusFactor > _radiusMin && radiusFactor < _radiusMax)
        {
            return Outcome;
        }

        return FloorGenerationOutcome.None;
    }
}