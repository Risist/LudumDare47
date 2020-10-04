using System.Collections.Generic;
using UnityEngine;

public class RandomDistributedPropsGenerationWidget : PropGenerationWidget
{
    [SerializeField] private List<GameObject> _possibleProps;
    [SerializeField] private RandomGeneratorSingleton _random;
    [SerializeField] private float _propsDensity;

    public override void AddProps(PropsSpecification propsSpecification, FloorSpecification floorSpecification)
    {
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
    }
}