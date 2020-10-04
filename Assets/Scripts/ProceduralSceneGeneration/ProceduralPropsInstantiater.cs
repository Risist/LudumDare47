using System.Linq;
using UnityEngine;

public class ProceduralPropsInstantiater : MonoBehaviour
{
    [SerializeField] private Transform _propsParent;

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

    public void InstantiateProps(PropsSpecification propsSpecification)
    {
        DestroyCurrentWalls();
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
                        new Vector3(x, 0, y) - new Vector3(centerOffset.x, 0, centerOffset.y) + new Vector3(definition.OffsetFromCenter.x, 0, definition.OffsetFromCenter.y);
                    prop.transform.localRotation = Quaternion.Euler(0, definition.Angle, 0);

                    prop.name = $"Prop-{x}-{y} - {definition.Prefab.name}";
                }
            }
        }
    }

}