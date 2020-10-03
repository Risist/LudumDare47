using System.Collections;
using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;

public class ProceduralSceneGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _floorSize;
    [SerializeField] private ProceduralFloorGenerator _floorGenerator;
    [SerializeField] private ProceduralFloorInstantiater _floorInstantiater;
    [SerializeField] private ProceduralWallsGenerator _wallsGenerator;
    [SerializeField] private ProceduralWallsInstantiater _wallsInstantiater;
    [SerializeField] private ProceduralPropsGenerator _prosGenerator;

    void Start()
    {
        Generate();
    }

    [ContextMenu(nameof(Generate))]
    void Generate()
    {
        var floorSpecification = _floorGenerator.GenerateFloorSpecification(_floorSize);
        _floorInstantiater.InstantiateFloor(floorSpecification, _floorSize);
        var wallsSpecification = _wallsGenerator.CreateWallsSpecification(floorSpecification);

        _wallsInstantiater.InstantiateWalls( new Vector2Int(floorSpecification.FloorPresenceArray.GetLength(0), floorSpecification.FloorPresenceArray.GetLength(1)), wallsSpecification);
        _prosGenerator.GenerateProps(floorSpecification);
    }
}