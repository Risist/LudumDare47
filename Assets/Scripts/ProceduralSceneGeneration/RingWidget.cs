using UnityEngine;

public class RingWidget : FloorGenerationWidget
{
    [SerializeField] private float _radiusMin;
    [SerializeField] private float _radiusMax;
    [SerializeField] private bool _isPresentIfConditionIsMet;

    public override bool? FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        var radius = queriedPosition.magnitude;
        var radiusFactor = 2*radius / roomSize.magnitude;

        if (radiusFactor > _radiusMin && radiusFactor < _radiusMax)
        {
            return _isPresentIfConditionIsMet;
        }

        return null;
    }
}