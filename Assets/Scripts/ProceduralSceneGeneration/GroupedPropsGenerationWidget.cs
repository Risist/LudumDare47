using System.Collections.Generic;
using UnityEngine;

public class GroupedPropsGenerationWidget : PropGenerationWidget
{
    [SerializeField] private List<GameObject> _possibleProps;
    [SerializeField] private RandomGeneratorSingleton _random; 
    [SerializeField] private float _propsGroupsDensity;
    [SerializeField] private Vector2 _propsDensityPerGroupRange;
    [SerializeField] private Vector2Int _groupRadiusRange;

    public override void AddProps(PropsSpecification propsSpecification, FloorSpecification floorSpecification)
    {
        var propsGroupsCount = Mathf.RoundToInt(floorSpecification.Size.x * floorSpecification.Size.y * _propsGroupsDensity);

        for (int i = 0; i < propsGroupsCount; i++)
        {
            var coords = new Vector2Int(_random.RandomInt(floorSpecification.Size.x),
                _random.RandomInt(floorSpecification.Size.y));

            var groupRadius = Mathf.CeilToInt(_random.RandomFloat(_groupRadiusRange.x, _groupRadiusRange.y));

            var countInGroup = _random.RandomFloat(_propsDensityPerGroupRange.x, _propsDensityPerGroupRange.y) * (groupRadius+1)*(groupRadius+1);

            for (int j = 0; j < countInGroup; j++)
            {
                var thisPropCoords = coords + new Vector2Int(_random.RandomInt(-groupRadius, groupRadius),
                    _random.RandomInt(-groupRadius, groupRadius));
                if (thisPropCoords.x >= 0 && thisPropCoords.y >= 0 && thisPropCoords.x < floorSpecification.Size.x &&
                    thisPropCoords.y < floorSpecification.Size.y)
                {
                    var angle = _random.RandomFloat(0, 360);
                    var propPrefab = _random.RandomElement(_possibleProps);
                    var offsetFromCenter = new Vector2(_random.RandomFloat(-0.5f,0.5f), _random.RandomFloat(-0.5f, 0.5f));

                    if (floorSpecification.FloorPresenceArray[thisPropCoords.x, thisPropCoords.y])
                    {
                        propsSpecification.SetDefinition(thisPropCoords,
                            new PropInstanceDefinition() {Angle = angle, Prefab = propPrefab, OffsetFromCenter = offsetFromCenter});
                    }
                }
            }
        }
    }
}