using System.Collections.Generic;
using UnityEngine;

public class ProceduralPropsGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleProps;
    [SerializeField] private RandomGeneratorSingleton _random;
    [SerializeField] private float _propsDensity;

    public PropsSpecification GeneratePropsSpecification(FloorSpecification floorSpecification)
    {
        var propsSpecification = new PropsSpecification(floorSpecification.Size);
        var propsCount = Mathf.RoundToInt(floorSpecification.Size.x * floorSpecification.Size.y * _propsDensity);

        for (int i = 0; i < propsCount; i++)
        {
            var coords = new Vector2Int(_random.RandomInt(floorSpecification.Size.x),
                _random.RandomInt(floorSpecification.Size.y));
            var angle = _random.RandomElement(new List<float>() {0, 90, 180, 270});
            var propPrefab = _random.RandomElement(_possibleProps);

            if (floorSpecification.FloorPresenceArray[coords.x, coords.y])
            {
                propsSpecification.SetDefinition(coords,
                    new PropInstanceDefinition() {Angle = angle, Prefab = propPrefab});
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
