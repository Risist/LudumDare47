using System.Linq;
using UnityEngine;

public class ProceduralFloorInstantiater : MonoBehaviour
{
    [SerializeField] private Transform _floorParent;
    [SerializeField] private GameObject _floorCellPrefab;

    private void DestroyCurrentFloor()
    {
        var floorChildren = _floorParent.GetComponentsInChildren<Component>().Select(c => c.gameObject).Where(c => c.gameObject != _floorParent.gameObject).Distinct()
            .ToList();
        foreach (var child in floorChildren)
        {
            GameObject.Destroy(child);
        }
    }

    public void InstantiateFloor(FloorSpecification floorSpecification, Vector2Int floorSize)
    {
        DestroyCurrentFloor();

        var centerOffset = floorSize / 2;

        for (int x = 0; x < floorSize.x; x++)
        {
            for (int y = 0; y < floorSize.y; y++)
            {
                if (floorSpecification.FloorPresenceArray[x, y])
                {
                    var cell = GameObject.Instantiate(_floorCellPrefab,
                        new Vector3(x, 0, y) - new Vector3(centerOffset.x, 0, centerOffset.y), Quaternion.identity,
                        _floorParent);
                    cell.name = $"Cell {x}-{y}";
                }
            }
        }
    }
}