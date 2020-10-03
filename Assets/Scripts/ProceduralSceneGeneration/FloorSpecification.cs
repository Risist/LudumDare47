public class FloorSpecification
{
    private bool[,] _floorPresenceArray;

    public FloorSpecification(bool[,] floorPresenceArray)
    {
        _floorPresenceArray = floorPresenceArray;
    }

    public bool[,] FloorPresenceArray => _floorPresenceArray;
}