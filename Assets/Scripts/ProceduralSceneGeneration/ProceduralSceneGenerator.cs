using System.Collections;
using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;

public class ProceduralSceneGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _floorSize;
    [SerializeField] private ProceduralFloorGenerator _floorGenerator;
    [SerializeField] private ProceduralFloorInstantiater _floorInstantiater;
    [SerializeField] private ProceduralWallsGenerator _wallsGenerator;

    void Start()
    {
        Generate();
    }

    [ContextMenu(nameof(Generate))]
    void Generate()
    {
        var floorSpecification = _floorGenerator.GenerateFloorSpecification(_floorSize);
        _floorInstantiater.InstantiateFloor(floorSpecification, _floorSize);
        _wallsGenerator.GenerateWalls(floorSpecification);
    }
}