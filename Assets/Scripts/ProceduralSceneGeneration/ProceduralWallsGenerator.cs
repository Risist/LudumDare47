using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ProceduralSceneGeneration
{
    public class ProceduralWallsGenerator : MonoBehaviour
    {
        [SerializeField] private List<OutsideWallWidget> _outsideWallWidgets;

        void Awake()
        {
            _outsideWallWidgets = GetComponents<OutsideWallWidget>().ToList();
        }

        public WallsSpecification CreateWallsSpecification(FloorSpecification floorSpecification)
        {
            var wallsSpecification = new WallsSpecification(new Vector2Int(floorSpecification.FloorPresenceArray.GetLength(0), floorSpecification.FloorPresenceArray.GetLength(1)));
            _outsideWallWidgets.ForEach(c => c.AddWalls(wallsSpecification, floorSpecification));
            return wallsSpecification;
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
