using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;


public class OutsideWallWidget : WallGenerationWidget
{
    public override void AddWalls(WallsSpecification spec, FloorSpecification floorSpecification)
    {
        var width = floorSpecification.FloorPresenceArray.GetLength(0);
        var height = floorSpecification.FloorPresenceArray.GetLength(1);

        for (var y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (floorSpecification.FloorPresenceArray[x, y])
                {
                    spec.AddWallDirection(new Vector2Int(x, y), WallDirection.Left);
                    break;
                }
            }
        }

        for (var y = 0; y < height; y++)
        {
            for (int x = width - 1; x >= 0; x--)
            {
                if (floorSpecification.FloorPresenceArray[x, y])
                {
                    spec.AddWallDirection(new Vector2Int(x, y), WallDirection.Right);
                    break;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (floorSpecification.FloorPresenceArray[x, y])
                {
                    spec.AddWallDirection(new Vector2Int(x, y), WallDirection.Down);
                    break;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (var y = height - 1; y >= 0; y--)
            {
                if (floorSpecification.FloorPresenceArray[x, y])
                {
                    spec.AddWallDirection(new Vector2Int(x, y), WallDirection.Up);
                    break;
                }
            }
        }
    }
}