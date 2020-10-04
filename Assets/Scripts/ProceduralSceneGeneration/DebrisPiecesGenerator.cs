using Assets.Scripts.ProceduralSceneGeneration;
using UnityEngine;

public class DebrisPiecesGenerator : MonoBehaviour
{
    [SerializeField] private float _floatHeight;
    [SerializeField] private float _rootMultiplier;
    [SerializeField] private DebrisPiece _debrisPiecePrefab;
    [SerializeField] private Transform _pullingCenter;
    [SerializeField] private Transform _debrisPiecesParent;
    [SerializeField] private RandomGeneratorSingleton _random;
    [SerializeField] private float _propsDensity;

    public void GeneratePieces(Vector2Int sceneSize)
    {
        var propsCount = sceneSize.x * sceneSize.y * _propsDensity;

        for (int i = 0; i < propsCount; i++)
        {
            var startPosition =
                new Vector3(_random.RandomFloat(-sceneSize.x/2f , sceneSize.x/2f )*_rootMultiplier, _floatHeight,
                    _random.RandomFloat(-sceneSize.y/2f , sceneSize.y/2f )*_rootMultiplier)+
                new Vector3(sceneSize.x * 0.25f, 0, sceneSize.y * 0.25f)*0;

            var debrisPiece = GameObject.Instantiate(_debrisPiecePrefab, _debrisPiecesParent).GetComponentNotNull<DebrisPiece>();
            debrisPiece.transform.localPosition = startPosition;
            debrisPiece.transform.localScale = Vector3.one *0.5f;
            debrisPiece.PullingCenter = _pullingCenter;
            debrisPiece.FloatHeight = _floatHeight;
        }
    }
}