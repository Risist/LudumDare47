using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitingCameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _rotationCenter;
    private Camera _camera;

    [Header("CameraHeight")] 
    [SerializeField] private CameraHeightMode _heightMode;
    [SerializeField] private float _constantHeight;
    [SerializeField] private float _heightByDistanceFactor;

    [Header("CameraHorizontalPosition")] 
    [SerializeField] private CameraHorizontalPositionMode _horizontalPositionMode;
    [SerializeField] private Vector2 _horizontalDistanceRange; 

    [Header("Location change speed")]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _transpositionSpeed;

    [Header("Fov settings in perspective mode")]
    [SerializeField] private bool _changeFovByDistanceToTarget;
    [SerializeField] private float _fovMultiplier;

    void Awake()
    {
        _camera = this.GetComponentNotNull<Camera>();
    }

    void Update()
    {
        if (_target == null)
        {
            return;
        }
        var targetAngle = Mathf.Atan2(_target.position.z - _rotationCenter.position.z, _target.position.x - _rotationCenter.position.x);
        var targetDistanceFromCenter = Vector2.Distance(new Vector2(_target.position.x, _target.position.z), new Vector2(_rotationCenter.position.x, _rotationCenter.position.z));

        var cameraHorizontalPosition = new Vector2(Mathf.Cos(targetAngle+Mathf.PI), Mathf.Sin(targetAngle+Mathf.PI)) * targetDistanceFromCenter;
        var positionMagnitude = cameraHorizontalPosition.magnitude;


        if (_horizontalPositionMode == CameraHorizontalPositionMode.ImitatingTargetDistanceFromCenter)
        {
            cameraHorizontalPosition = cameraHorizontalPosition.normalized * Mathf.Clamp( positionMagnitude, _horizontalDistanceRange.x, _horizontalDistanceRange.y);
        }
        else
        {
            var horizontalCenterToTarget = new Vector2(_target.position.x, _target.position.z) - new Vector2(_rotationCenter.position.x, _rotationCenter.position.z);
            var horizontalCenterToTargetDir = horizontalCenterToTarget.normalized;
            cameraHorizontalPosition = new Vector2(_target.position.x, _target.position.z) - horizontalCenterToTargetDir * _horizontalDistanceRange[0];
        }

        var cameraHeight = _constantHeight;
        if (_heightMode == CameraHeightMode.DependingOnTargetDistanceFromCenter)
        {
            cameraHeight = _constantHeight + targetDistanceFromCenter * _heightByDistanceFactor;
        }

        transform.position = Vector3.Lerp(transform.position,
            new Vector3(cameraHorizontalPosition.x, cameraHeight, cameraHorizontalPosition.y), _transpositionSpeed * Time.deltaTime);

        var cameraToTargetDirection = (_target.position - _camera.transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraToTargetDirection), _rotationSpeed*Time.deltaTime);

        if (_changeFovByDistanceToTarget)
        {
            var distance = Vector3.Distance(_target.position, transform.position);
            _camera.fieldOfView = _fovMultiplier / Mathf.Pow(distance, 0.66f);
        }
    }

    public enum CameraHeightMode
    {
        Constant=0, DependingOnTargetDistanceFromCenter=1
    }

    public enum CameraHorizontalPositionMode
    {
        ConstantDistanceFromTarget=0, ImitatingTargetDistanceFromCenter=1
    }
}