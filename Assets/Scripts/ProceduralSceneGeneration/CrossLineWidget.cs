using UnityEngine;

public class CrossLineWidget : FloorGenerationWidget
{
    [SerializeField] private int _lineWidth;
    [SerializeField] private bool _isPresentIfConditionIsMet;

    public override bool? FloorIsPresent(Vector2Int queriedPosition, Vector2Int roomSize)
    {
        if (Mathf.Abs(queriedPosition.x) < _lineWidth || Mathf.Abs(queriedPosition.y) < _lineWidth)
        {
            return _isPresentIfConditionIsMet;
        }

        return null;
    }
}