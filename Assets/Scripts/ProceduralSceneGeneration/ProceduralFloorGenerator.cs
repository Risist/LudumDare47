using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProceduralFloorGenerator : MonoBehaviour
{
    [SerializeField] private List<FloorGenerationWidget> _widgets;

    public FloorSpecification GenerateFloorSpecification(Vector2Int size)
    {
        _widgets = GetComponents<FloorGenerationWidget>().Where(c=>c.enabled).ToList();
        var presenceArray = new bool[size.x, size.y];
        var center = size / 2;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var offsetPosition = new Vector2Int(x,y) - center;
                foreach (var widget in _widgets)
                {
                    var outcome = widget.FloorIsPresent(offsetPosition, size);
                    switch (outcome)
                    {
                        case FloorGenerationWidget.FloorGenerationOutcome.None:
                            break;
                        case FloorGenerationWidget.FloorGenerationOutcome.On:
                            presenceArray[x, y] = true;
                            break;
                        case FloorGenerationWidget.FloorGenerationOutcome.Off:
                            presenceArray[x, y] = false;
                            break;
                        case FloorGenerationWidget.FloorGenerationOutcome.Flip:
                            presenceArray[x, y] = !presenceArray[x,y];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        return new FloorSpecification(presenceArray);
    }
}