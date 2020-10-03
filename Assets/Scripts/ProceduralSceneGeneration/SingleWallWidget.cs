using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;

public class SingleWallWidget : WallGenerationWidget
{
    public Vector2 _startPoint;
    public Vector2 _endPoint;

    public override void AddWalls(WallsSpecification specification, FloorSpecification floorSpecification)
    {
        var size = floorSpecification.Size;

        var start = ClampCoords(floorSpecification.Size, new Vector2Int(Mathf.RoundToInt(_startPoint.x* size.x), Mathf.RoundToInt(_startPoint.y*size.y)));
        var end = ClampCoords(floorSpecification.Size, new Vector2Int(Mathf.RoundToInt(_endPoint.x* size.x), Mathf.RoundToInt(_endPoint.y*size.y)));

        var points = GetPointsOnLine(start.x, start.y, end.x, end.y).ToList();

        for (int i = 0; i < points.Count - 1; i++)
        {
            var p1 = points[i];
            var p2 = points[i+1];

            if (p1.x == p2.x)
            {
                //do nothing
            }
            else if (p1.x+1 == p2.x)
            {
                TryAddingWall(new Vector2Int(p1.x, p1.y),WallDirection.Up,specification, floorSpecification );
            }else if (p1.x - 1 == p2.x)
            {
                TryAddingWall(new Vector2Int(p1.x-1, p1.y),WallDirection.Up,specification, floorSpecification  );
            }
            else
            {
                MyAssert.Fail($"x delta is too big {p1} {p2}");
            }

            if (p1.y == p2.y)
            {
                //do nothing
            }
            else if (p1.y+1 == p2.y)
            {
                TryAddingWall(new Vector2Int(p1.x, p1.y+1),WallDirection.Right,specification, floorSpecification );
            }else if (p1.y - 1 == p2.y)
            {
                TryAddingWall(new Vector2Int(p1.x, p1.y),WallDirection.Right, specification, floorSpecification );
            }
            else
            {
                MyAssert.Fail($"y delta is too big {p1} {p2}");
            }
        }
    }

    private Vector2Int ClampCoords(Vector2Int size, Vector2Int p)
    {
        return new Vector2Int(
            Mathf.Min(size.x-1, Mathf.Max(0, p.x)),
            Mathf.Min(size.y-1, Mathf.Max(0, p.y))
            );
    }

    private void TryAddingWall(Vector2Int coords, WallDirection direction, WallsSpecification wallsSpecification,
        FloorSpecification floorSpecification)
    {
        Vector2Int additionalCoordOffset;
        switch (direction)
        {
            case WallDirection.Up:
                additionalCoordOffset = new Vector2Int(0, 1);
                break;
            case WallDirection.Down:
                additionalCoordOffset = new Vector2Int(0, -1);
                break;
            case WallDirection.Left:
                additionalCoordOffset = new Vector2Int(-1, 0);
                break;
            case WallDirection.Right:
                additionalCoordOffset = new Vector2Int(1, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        try
        {
            var offsetedCoords = coords + additionalCoordOffset;
            if (floorSpecification.FloorPresenceArray[coords.x, coords.y] || (
                offsetedCoords.x >= 0 && offsetedCoords.y >= 0 && offsetedCoords.x < floorSpecification.Size.x &&
                offsetedCoords.y < floorSpecification.Size.y &&
                floorSpecification.FloorPresenceArray[offsetedCoords.x, offsetedCoords.y]
            ))
            {
                wallsSpecification.AddWallDirection(coords, direction);
            }
        }
        catch
        {
            throw;
        }
    }


    public static IEnumerable<(int x, int y)> GetPointsOnLine(int x0, int y0, int x1, int y1)
    {
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            int t;
            t = x0; // swap x0 and y0
            x0 = y0;
            y0 = t;
            t = x1; // swap x1 and y1
            x1 = y1;
            y1 = t;
        }
        if (x0 > x1)
        {
            int t;
            t = x0; // swap x0 and x1
            x0 = x1;
            x1 = t;
            t = y0; // swap y0 and y1
            y0 = y1;
            y1 = t;
        }
        int dx = x1 - x0;
        int dy = Math.Abs(y1 - y0);
        int error = dx / 2;
        int ystep = (y0 < y1) ? 1 : -1;
        int y = y0;
        for (int x = x0; x <= x1; x++)
        {
            yield return((steep ? y : x), (steep ? x : y));
            error = error - dy;
            if (error < 0)
            {
                y += ystep;
                error += dx;
            }
        }
        yield break;
    }
}