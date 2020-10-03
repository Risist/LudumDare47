using UnityEngine;

public class SquareRingWidget : FloorGenerationWidget
{
    [SerializeField] private float _radiusMin;
    [SerializeField] private float _radiusMax;
    [SerializeField] private bool _isPresentIfConditionIsMet;

    public override bool? FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        var radiuses = new Vector2(2f*queriedPosition.x / roomSize.x, 2f*queriedPosition.y /roomSize.y);

        var radiusFactor = Mathf.Max(Mathf.Abs(radiuses.x), Mathf.Abs(radiuses.y));

        if (radiusFactor > _radiusMin && radiusFactor < _radiusMax)
        {
            return _isPresentIfConditionIsMet;
        }

        return null;
    }
}