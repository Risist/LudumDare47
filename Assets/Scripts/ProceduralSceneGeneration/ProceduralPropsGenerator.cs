using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ProceduralPropsGenerator : MonoBehaviour
{
    [SerializeField] private List<PropGenerationWidget> _propGenerators;

    public PropsSpecification GeneratePropsSpecification(FloorSpecification floorSpecification)
    {
        _propGenerators = GetComponents<PropGenerationWidget>().ToList();
        var specification = new PropsSpecification(floorSpecification.Size);
        _propGenerators.ForEach(c => c.AddProps(specification, floorSpecification));
        return specification;
    }
}

public abstract class PropGenerationWidget : MonoBehaviour
{
    public abstract void AddProps(PropsSpecification specification, FloorSpecification floorSpecification);
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
    public Vector2 OffsetFromCenter;
}