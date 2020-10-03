using System.Collections;
using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;

public class ProceduralSceneGenerator : MonoBehaviour
{
    public Vector2Int _floorSize;
    [SerializeField] private int _blackHoleAreaSize;
    public ProceduralFloorGenerator _floorGenerator;
    public ProceduralFloorInstantiater _floorInstantiater;
    public ProceduralWallsGenerator _wallsGenerator;
    public  ProceduralWallsInstantiater _wallsInstantiater;
    public  ProceduralPropsGenerator _propsGenerator;
    public  ProceduralPropsInstantiater _propsInstantiater;

    void Start()
    {
        //Generate();
    }

    [ContextMenu(nameof(Generate))]
    public void Generate()
    {
        var floorSpecification = _floorGenerator.GenerateFloorSpecification(_floorSize);
        var wallsSpecification = _wallsGenerator.CreateWallsSpecification(floorSpecification);
        var propsSpecification = _propsGenerator.GeneratePropsSpecification(floorSpecification);

        ClearOutBlackHoleArea(floorSpecification, wallsSpecification, propsSpecification);

        _floorInstantiater.InstantiateFloor(floorSpecification, _floorSize);
        _wallsInstantiater.InstantiateWalls( new Vector2Int(floorSpecification.FloorPresenceArray.GetLength(0), floorSpecification.FloorPresenceArray.GetLength(1)), wallsSpecification);
        _propsInstantiater.InstantiateProps(propsSpecification);
        AstarPath.active.Scan();
    }

    private void ClearOutBlackHoleArea(FloorSpecification floorSpecification, WallsSpecification wallsSpecification, PropsSpecification propsSpecification)
    {
        var size = floorSpecification.Size;
        var center = size / 2;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var distanceToCenter = Vector2.Distance(new Vector2(x, y), center);
                if (distanceToCenter < _blackHoleAreaSize)
                {
                    floorSpecification.FloorPresenceArray[x, y] = true;
                    wallsSpecification.EmptyWalls(new Vector2Int(x,y));
                    propsSpecification.SetDefinition(new Vector2Int(x, y), null);
                }
            }
        }

        for (int x = -_blackHoleAreaSize / 2 ; x < _blackHoleAreaSize / 2  ; x++)
        {
            wallsSpecification.AddWallDirection(center + new Vector2Int(x,-_blackHoleAreaSize+2), WallDirection.Down);
            wallsSpecification.AddWallDirection(center + new Vector2Int(x,_blackHoleAreaSize-2), WallDirection.Up);
        }

        for (int y = -_blackHoleAreaSize / 2 ; y < _blackHoleAreaSize / 2 ; y++)
        {
            wallsSpecification.AddWallDirection(center + new Vector2Int(-_blackHoleAreaSize+2,y), WallDirection.Left);
            wallsSpecification.AddWallDirection(center + new Vector2Int(_blackHoleAreaSize-2,y ), WallDirection.Right);
        }
    }
}