﻿using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ProceduralSceneGeneration
{
    public class ProceduralWallsInstantiater : MonoBehaviour
    { 
        [SerializeField] private Transform _wallsParent;
        [SerializeField] private Transform _indestructibleWallsPrefab;
        [SerializeField] private Transform _destructibleWallPrefab;

        private void DestroyCurrentWalls()
        {
            var wallChildren = _wallsParent.GetComponentsInChildren<Component>().Select(c => c.gameObject)
                .Where(c => c.gameObject != _wallsParent.gameObject).Distinct()
                .ToList();
            foreach (var child in wallChildren)
            {
                GameObject.Destroy(child);
            }
        }

        public void InstantiateWalls(Vector2Int size, WallsSpecification wallsSpecification)
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

                        var position = new Vector3(x, 0, y) - new Vector3(center.x, 0, center.y);
                        var rotation = Quaternion.Euler(0, angle, 0);

                        Transform prefab = wallsSpecification.GetIsDestructible(new Vector2Int(x, y))
                            ? _destructibleWallPrefab
                            : _indestructibleWallsPrefab;

                        var wall = GameObject.Instantiate(prefab, position, rotation, _wallsParent);
                        wall.transform.localPosition = position;
                        wall.transform.localRotation = rotation;
                        wall.name = $"Wall {x}-{y}-{aDirection}-{prefab.name}";
                    }
                }
            }

        }

    }
}