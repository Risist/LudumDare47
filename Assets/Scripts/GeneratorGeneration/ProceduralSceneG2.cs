using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GeneratorGeneration
{
    [DefaultExecutionOrder(-999999999)]
    public class ProceduralSceneG2 : MonoBehaviour
    {
        [SerializeField] private GameObject _waterTile;
        [SerializeField] private RandomGeneratorSingleton _random;
        [SerializeField] private ProceduralSceneGenerator _sceneGenerator;
        [SerializeField] private Vector2 _floorWidgetsCountRange;
        [SerializeField] private Vector2 _wallsCountRange;
        [SerializeField] private Vector2 _wallsLengthRange;
        [SerializeField] private Vector2 _orbitingWallsCountRange;
        [SerializeField] private Vector2 _orbitingWallsAngleLengthRange;
        [SerializeField] private Vector2Int _floorSizeRange;

        void Start()
        {
            CreateProceduralScene();
        }

        [ContextMenu(nameof(CreateProceduralScene))]
        public void CreateProceduralScene()
        {
            var floorWidgetsCount = Mathf.RoundToInt(_random.RandomFloat(_floorWidgetsCountRange.x, _floorWidgetsCountRange.y));
            var floorWidgetsPossibilities = new List<Action<GameObject>>()
            {
                (g) => {
                    var widget = g.AddComponent<CrossLineWidget>();
                    RandomizeCrossLineWidget(widget);
                },
                (g) => {
                    var widget = g.AddComponent<RingWidget>();
                    RandomizeRingWidget(widget);
                },
                (g) => {
                    var widget = g.AddComponent<SquareRingWidget>();
                    RandomizeSquareRingWidget(widget);
                }
            };

            for (int i = 0; i < floorWidgetsCount; i++)
            {
                _random.RandomElement(floorWidgetsPossibilities).Invoke(_sceneGenerator._floorGenerator.gameObject);
            }

            var wallsCount = Mathf.RoundToInt(_random.RandomFloat(_wallsCountRange.x, _wallsCountRange.y));
            for (int i = 0; i < wallsCount; i++)
            {
                var wallLength = _random.RandomFloat(_wallsLengthRange.x, _wallsLengthRange.y);
                var startPoint = new Vector2(_random.RandomFloat(0, 1), _random.RandomFloat(0, 1));
                var angle = _random.RandomFloat(Mathf.PI * 2);
                var endPoint = startPoint + new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * wallLength;
                var wallWidget = _sceneGenerator._wallsGenerator.gameObject.AddComponent<SingleWallWidget>();
                wallWidget._startPoint = startPoint;
                wallWidget._endPoint = endPoint;
            }


            var orbitingWallsCount = Mathf.RoundToInt(_random.RandomFloat(_orbitingWallsCountRange.x, _orbitingWallsCountRange.y));
            for (int i = 0; i < orbitingWallsCount; i++)
            {
                var angleLength = _random.RandomFloat(_orbitingWallsAngleLengthRange.x, _orbitingWallsAngleLengthRange.y);
                var radius = _random.RandomFloat(0.2f, 0.5f);
                var startAngle = _random.RandomFloat(Mathf.PI * 2);
                var startPoint = new Vector2(Mathf.Sin(startAngle), Mathf.Cos(startAngle)) *radius+ new Vector2(0.5f, 0.5f);

                var endAngle = startAngle + angleLength;

                var endPoint = new Vector2(Mathf.Sin(endAngle), Mathf.Cos(endAngle)) *radius+ new Vector2(0.5f, 0.5f);
                var wallWidget = _sceneGenerator._wallsGenerator.gameObject.AddComponent<SingleWallWidget>();
                wallWidget._startPoint = startPoint;
                wallWidget._endPoint = endPoint;
            }

            _sceneGenerator._floorSize = new Vector2Int(_random.RandomInt(_floorSizeRange.x ,_floorSizeRange.y), _random.RandomInt(_floorSizeRange.x ,_floorSizeRange.y) );
            _sceneGenerator.Generate();

            _waterTile.transform.localScale = new Vector3(_sceneGenerator._floorSize.x * 0.02985f, 1, _sceneGenerator._floorSize.y*0.02985f);
        }

        private void RandomizeCrossLineWidget(CrossLineWidget widget)
        {
            widget.Outcome = GetRandomGenerationOutcome();
            widget._lineWidth = _random.RandomInt(1, 4);
        }

        private FloorGenerationWidget.FloorGenerationOutcome GetRandomGenerationOutcome()
        {
            var r = _random.RandomFloat(1);
            if (r < 0.4)
            {
                return FloorGenerationWidget.FloorGenerationOutcome.Off;
            }

            if (r < 0.8)
            {
                return FloorGenerationWidget.FloorGenerationOutcome.On;
            }

            return FloorGenerationWidget.FloorGenerationOutcome.Flip;
        }

        private void RandomizeRingWidget(RingWidget widget)
        {
            widget.Outcome = GetRandomGenerationOutcome();
            var radiusMin = 0.2f + _random.RandomFloat(0.7f);
            var radiusWidth = radiusMin + _random.RandomFloat(0.15f);
            widget._radiusMin = radiusMin;
            widget._radiusMax =  radiusWidth;
        }

        private void RandomizeSquareRingWidget(SquareRingWidget widget)
        {
            widget.Outcome = GetRandomGenerationOutcome();
            var radiusMin = 0.2f + _random.RandomFloat(0.7f);
            var radiusWidth =  _random.RandomFloat(0.2f);
            widget._radiusMin = radiusMin;
            widget._radiusMax = radiusMin + radiusWidth;
        }
    }
}
