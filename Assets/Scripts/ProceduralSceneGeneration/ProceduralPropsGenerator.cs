using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralPropsGenerator : MonoBehaviour
{
    [SerializeField] private Transform _propsParent;
    [SerializeField] private RandomGeneratorSingleton _random;
    [SerializeField] private List<GameObject> _possibleProps;
    [SerializeField] private float _propsDensity;

    private void DestroyCurrentWalls()
    {
        var wallChildren = _propsParent.GetComponentsInChildren<Component>().Select(c => c.gameObject)
            .Where(c => c.gameObject != _propsParent.gameObject).Distinct()
            .ToList();
        foreach (var child in wallChildren)
        {
            GameObject.Destroy(child);
        }
    }

    public void GenerateProps(FloorSpecification floorSpecification)
    {
        DestroyCurrentWalls();
        var propsSpecification = GeneratePropsSpecification(floorSpecification);
        InstantiateProps(propsSpecification);
    }

    private PropsSpecification GeneratePropsSpecification(FloorSpecification floorSpecification)
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

    private void InstantiateProps(PropsSpecification propsSpecification)
    {
        var size = propsSpecification.Size;
        var centerOffset = size / 2;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var definition = propsSpecification.Props[x, y];
                if (definition != null)
                {
                    var prop = GameObject.Instantiate(definition.Prefab, _propsParent);
                    prop.transform.localPosition =
                        new Vector3(x, 0, y) - new Vector3(centerOffset.x, 0, centerOffset.y);
                    prop.transform.localRotation = Quaternion.Euler(0, definition.Angle, 0);

                    prop.name = $"Prop-{x}-{y} - {definition.Prefab.name}";
                }
            }
        }
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
