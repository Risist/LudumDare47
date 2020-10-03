using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ProceduralFloorGenerator : MonoBehaviour
{
    [SerializeField] private List<FloorGenerationWidget> _widgets;

    void Awake()
    {
        _widgets = GetComponents<FloorGenerationWidget>().ToList();
    }

    public FloorSpecification GenerateFloorSpecification(Vector2Int size)
    {
        var presenceArray = new bool[size.x, size.y];
        var center = size / 2;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var offsetPosition = new Vector2Int(x,y) - center;
                foreach (var widget in _widgets)
                {
                    var presence = widget.FloorIsPresent(offsetPosition, size);
                    if (presence.HasValue)
                    {
                        presenceArray[x, y] = presence.Value;
                    }
                }
            }
        }

        return new FloorSpecification(presenceArray);
    }
}