using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ProceduralSceneGeneration
{
    public class ProceduralWallsGenerator : MonoBehaviour
    {
        [SerializeField] private List<WallGenerationWidget> _outsideWallWidgets;

        public WallsSpecification CreateWallsSpecification(FloorSpecification floorSpecification)
        {
            _outsideWallWidgets = GetComponents<WallGenerationWidget>().ToList();
            var wallsSpecification = new WallsSpecification(new Vector2Int(floorSpecification.FloorPresenceArray.GetLength(0), floorSpecification.FloorPresenceArray.GetLength(1)));
            _outsideWallWidgets.ForEach(c => c.AddWalls(wallsSpecification, floorSpecification));
            return wallsSpecification;
        }
    }

    public class WallsSpecification
    {
        private List<WallDirection>[,] _walls;
        private bool[,] _isDestructible;

        public WallsSpecification(Vector2Int size)
        {
            _walls = new List<WallDirection>[size.x, size.y];
            _isDestructible = new bool[size.x,size.y];
            for(int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    _isDestructible[x, y] = true;
                }
            }
        }

        public void AddWallDirection(Vector2Int coords, WallDirection direction, bool isDestructible)
        {
            if (_walls[coords.x, coords.y] == null)
            {
                _walls[coords.x, coords.y] = new List<WallDirection>();
            }

            if (!_walls[coords.x, coords.y].Contains(direction))
            {
                _walls[coords.x, coords.y].Add(direction);
            }

            _isDestructible[coords.x, coords.y] &= isDestructible;
        }

        public List<WallDirection> GetWallDirections(Vector2Int coords)
        {
            if (_walls[coords.x, coords.y] == null)
            {
                return new List<WallDirection>();
            }

            return _walls[coords.x, coords.y];
        }

        public bool GetIsDestructible(Vector2Int coords)
        {
            return _isDestructible[coords.x, coords.y];
        }

        public void EmptyWalls(Vector2Int coords)
        {
            _walls[coords.x, coords.y] = null;
        }
    }

    public enum WallDirection
    {
        Up=1,Down=2,Left=3,Right=4
    } 
}
