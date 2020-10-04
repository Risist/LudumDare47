using UnityEngine;
using System.Collections;

public class Tongue : MonoBehaviour
{
    [Range(0, 1)] public float tongueSpeedFactor;
    [Range(0, 1)] public float tongueSpeedFactorBack;
    public float pullForce; 
    public float objectPullForce;
    public float maxReach;
    public float backDistanceTollerance;
    public Rigidbody _myRigidbody;
    Rigidbody _otherRigidbody;
    InputHolder _inputHolder;
    Collider _collider;
    float initialY;

    public enum EState
    {
        EIdle,
        EMoveForward,
        EMoveBackward,
        EPull
    }
    public EState currentState = EState.EIdle;
    bool inIdle;

    Vector3 _pullPosition;
    Timer tPull = new Timer(1.0f);

    private void Start()
    {
        _inputHolder = GetComponentInParent<InputHolder>();
        _collider = GetComponentInParent<Collider>();
        initialY = transform.localPosition.y;
        InitIdle();
    }

    bool pressed = false;
    private void Update()
    {
        if (_inputHolder.keys[1])
        {
            if(!pressed)
                InitMoveForward();
        }
        pressed = _inputHolder.keys[1];
    }

    private void FixedUpdate()
    {
        Debug.Log(currentState);
        switch(currentState)
        {
            case EState.EIdle:
                Idle();
                break;
            case EState.EMoveForward:
                MoveForward();
                break;
            case EState.EMoveBackward:
                MoveBackward();
                break;
            case EState.EPull:
                Pull();
                break;

        }
    }

    void InitIdle()
    {
        currentState = EState.EIdle;
        transform.localPosition = Vector3.zero;
        _collider.enabled = false;
    }
    public void InitMoveBackward()
    {
        if (currentState == EState.EIdle)
            return;
        if (transform.localPosition.ToPlane().magnitude <= 0.1f)
            InitIdle();
        else if(!inIdle)
        {
            currentState = EState.EMoveBackward;
        }
        _collider.enabled = false;
    }
    public void InitMoveForward()
    {
        if (currentState == EState.EPull)
            return;
        

        currentState = EState.EMoveForward; 
        _collider.enabled = true;
    }
    public void InitPull(Rigidbody otherRigidbody)
    {
        if (currentState == EState.EPull)
            return;
        _otherRigidbody = otherRigidbody;
        _pullPosition = transform.position;
        currentState = EState.EPull;
        tPull.Restart();
        _collider.enabled = true;
    }


    void Idle()
    {
        transform.localPosition = Vector3.zero;
    }
    void MoveForward()
    {
        var dist = (transform.localPosition - new Vector3(0, 0, maxReach)).ToPlane().magnitude;
        if (dist < backDistanceTollerance)
        {
            _inputHolder.rotationInput = Vector2.zero;
            InitMoveBackward();
            return;
        }
        _inputHolder.rotationInput = _inputHolder.directionInput;
        transform.localPosition = Vector3.Lerp(new Vector3(0, 0, maxReach), transform.localPosition, tongueSpeedFactor);
        transform.localPosition = transform.localPosition.ToPlane() + Vector3.up * initialY;
    }
    void MoveBackward()
    {
        transform.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), transform.localPosition, tongueSpeedFactorBack);
    }
    void Pull()
    {
        var dist = (transform.localPosition - new Vector3(0, 0, 0)).magnitude;
        if ( !_inputHolder.keys[1] && (dist < backDistanceTollerance || tPull.IsReady()) )
        {
            InitMoveBackward();
            return;
        }

        transform.position = _pullPosition;
        Vector3 toTongue = _pullPosition - _myRigidbody.position;
        _myRigidbody.AddForce(toTongue * pullForce);

        if(_otherRigidbody)
        {
            toTongue = _pullPosition - _otherRigidbody.position;
            _myRigidbody.AddForce(toTongue * objectPullForce);
        }
        transform.localPosition = transform.localPosition.ToPlane() + Vector3.up * initialY;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody == _myRigidbody)
            return;

        if (other.isTrigger)
            return;

        if (currentState == EState.EMoveForward)
        {
            InitPull(other.attachedRigidbody);
        }
    }
}