using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ProceduralSceneGeneration
{
    public class ProceduralWallsGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _wallsParent;
        [SerializeField] private Transform _wallPrefab;

        public void GenerateWalls(FloorSpecification floorSpecification)
        {
            var wallsSpecification = CreateWallsSpecification(floorSpecification);
            InstantiateWalls(new Vector2Int(floorSpecification.FloorPresenceArray.GetLength(0), floorSpecification.FloorPresenceArray.GetLength(1)), wallsSpecification);
        }

        private void InstantiateWalls(Vector2Int size, WallsSpecification wallsSpecification)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var directions = wallsSpecification.GetWallDirections(new Vector2Int(x, y));

                    foreach (var aDirection in directions)
                    {
                        float angle;
                        switch (aDirection)
                        {
                            case WallDirection.Up:
                                angle = 90;
                                break;
                            case WallDirection.Down:
                                angle = -90;
                                break;
                            case WallDirection.Left:
                                angle = 0;
                                break;
                            case WallDirection.Right:
                                angle = 180;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        var center = size / 2;

                        var wall = GameObject.Instantiate(_wallPrefab,
                            new Vector3(x, 0, y) - new Vector3(center.x, 0, center.y), Quaternion.Euler(0, angle, 0),
                            _wallsParent);
                        wall.name = $"Wall {x}-{y}-{aDirection}";
                    }
                }
            }

        }

        private WallsSpecification CreateWallsSpecification(FloorSpecification floorSpecification)
        {
            var width = floorSpecification.FloorPresenceArray.GetLength(0);
            var height = floorSpecification.FloorPresenceArray.GetLength(1);
            var spec = new WallsSpecification(new Vector2Int(width, height));

            for (var y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (floorSpecification.FloorPresenceArray[x, y])
                    {
                        spec.AddWallDirection(new Vector2Int(x,y), WallDirection.Left );
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

            return spec;
        }
    }

    public class WallsSpecification
    {
        private List<WallDirection>[,] _walls;

        public WallsSpecification(Vector2Int size)
        {
            _walls = new List<WallDirection>[size.x, size.y];
        }

        public void AddWallDirection(Vector2Int coords, WallDirection direction)
        {
            if (_walls[coords.x, coords.y] == null)
            {
                _walls[coords.x, coords.y] = new List<WallDirection>();
            }

            if (!_walls[coords.x, coords.y].Contains(direction))
            {
                _walls[coords.x, coords.y].Add(direction);
            }
        }

        public List<WallDirection> GetWallDirections(Vector2Int coords)
        {
            if (_walls[coords.x, coords.y] == null)
            {
                return new List<WallDirection>();
            }

            return _walls[coords.x, coords.y];
        }
    }

    public enum WallDirection
    {
        Up=1,Down=2,Left=3,Right=4
    } 
}
