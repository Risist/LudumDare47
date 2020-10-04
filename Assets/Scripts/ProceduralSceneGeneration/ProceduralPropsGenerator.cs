using System.Collections.Generic;
using UnityEngine;

public class ProceduralPropsGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleProps;
    [SerializeField] private RandomGeneratorSingleton _random;
    [SerializeField] private float _propsGroupsDensity;
    [SerializeField] private Vector2 _propsDensityPerGroupRange;
    [SerializeField] private Vector2Int _groupRadiusRange;

    public PropsSpecification GeneratePropsSpecification(FloorSpecification floorSpecification)
    {
        var propsSpecification = new PropsSpecification(floorSpecification.Size);
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

                    var angle = _random.RandomElement(new List<float>() {0, 90, 180, 270});
                    var propPrefab = _random.RandomElement(_possibleProps);

                    if (floorSpecification.FloorPresenceArray[thisPropCoords.x, thisPropCoords.y])
                    {
                        propsSpecification.SetDefinition(thisPropCoords,
                            new PropInstanceDefinition() {Angle = angle, Prefab = propPrefab});
                    }
                }
            }
        }

        return propsSpecification;
    }

}

public class PropsSpecification
{
    private PropInstanceDefinition[,] _props;

    public PropsSpecification(Vector2Int size)
    {
        _props = new PropInstanceDefinition[size.x,size.y];
    }

    public void SetDefinition(Vector2Int coords, PropInstanceDefinition definition)
    {
        _props[coords.x, coords.y] = definition;
    }

    public PropInstanceDefinition[,] Props => _props;

    public Vector2Int Size => new Vector2Int(_props.GetLength(0), _props.GetLength(1));
}

public class PropInstanceDefinition
{
    public GameObject Prefab;
    public float Angle;
}
