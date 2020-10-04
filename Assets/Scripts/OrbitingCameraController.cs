using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitingCameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _rotationCenter;
    [SerializeField] private float _cameraHeight;
    [SerializeField] private bool _changeFovByDistanceToTarget;
    [SerializeField] private float _maximumCameraHorizontalDistanceFromCenter;
    [SerializeField] private float _fovMultiplier;
    [SerializeField] private bool _keepConstantHorizontalDistanceFromTarget;
    [SerializeField] private float _constantHorizontalDistanceFromTarget;
    [SerializeField] private float _backOffset;
    private Camera _camera;

    void Awake()
    {
        _camera = this.GetComponentNotNull<Camera>();
    }

    void Update()
    {
        var targetAngle = Mathf.Atan2(_target.position.z - _rotationCenter.position.z, _target.position.x - _rotationCenter.position.x);
        var targetDistanceFromCenter = Vector2.Distance(new Vector2(_target.position.x, _target.position.z), new Vector2(_rotationCenter.position.x, _rotationCenter.position.z));

        var cameraHorizontalPosition = new Vector2(Mathf.Cos(targetAngle+Mathf.PI), Mathf.Sin(targetAngle+Mathf.PI)) * targetDistanceFromCenter;
        var positionMagnitude = cameraHorizontalPosition.magnitude;
        cameraHorizontalPosition = cameraHorizontalPosition.normalized * Mathf.Min(_maximumCameraHorizontalDistanceFromCenter, positionMagnitude);

        if (_keepConstantHorizontalDistanceFromTarget)
        {
            var horizontalCenterToTarget = new Vector2(_target.position.x, _target.position.z) - new Vector2(_rotationCenter.position.x, _rotationCenter.position.z);
            var horizontalCenterToTargetDir = horizontalCenterToTarget.normalized;
            cameraHorizontalPosition = new Vector2(_target.position.x, _target.position.z) - horizontalCenterToTargetDir * _constantHorizontalDistanceFromTarget;
        }


        Vector3 toTarget = (_rotationCenter.position - _target.transform.position).ToPlane();
        transform.position = new Vector3(cameraHorizontalPosition.x, _cameraHeight, cameraHorizontalPosition.y) - toTarget.normalized * _backOffset;

        transform.LookAt(_target);

        if (_changeFovByDistanceToTarget)
        {
            var distance = Vector3.Distance(_target.position, transform.position);
            _camera.fieldOfView = _fovMultiplier / Mathf.Pow(distance, 0.66f);
        }
    }
}