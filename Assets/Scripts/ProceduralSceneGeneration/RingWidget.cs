using UnityEngine;

public class RingWidget : FloorGenerationWidget
{
    public float _radiusMin;
    public float _radiusMax;
    public FloorGenerationOutcome Outcome;

    public override FloorGenerationOutcome FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        var radius = queriedPosition.magnitude;
        var radiusFactor = 2*radius / roomSize.magnitude;

        if (radiusFactor > _radiusMin && radiusFactor < _radiusMax)
        {
            return Outcome;
        }

        return FloorGenerationOutcome.None;
    }
}