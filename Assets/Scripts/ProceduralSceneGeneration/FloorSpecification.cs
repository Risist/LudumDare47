using UnityEngine;

public class FloorSpecification
{
    private bool[,] _floorPresenceArray;

    public FloorSpecification(bool[,] floorPresenceArray)
    {
        _floorPresenceArray = floorPresenceArray;
    }

    public bool[,] FloorPresenceArray => _floorPresenceArray;

    public Vector2Int Size => new Vector2Int(_floorPresenceArray.GetLength(0), _floorPresenceArray.GetLength(1));
}