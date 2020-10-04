using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ProceduralSceneGeneration
{
    [RequireComponent(typeof(Rigidbody))]
    public class DebrisPiece : MonoBehaviour
    {
        public Transform PullingCenter;
        public float FloatHeight;
        public float FloatForceStrength;
        public float WaveAmplitude;
        public float PositionOffsetFrequency;
        public float WaveFrequency;
        public float RoundingForce;
        public float PullingForceFactor;
        private Rigidbody _rb;

        void Awake()
        {
            _rb = this.GetComponentNotNull<Rigidbody>();
        }

        void FixedUpdate()
        {
            var positionDelta = new Vector2(_rb.position.x, _rb.position.z) - new Vector2(PullingCenter.position.x, PullingCenter.position.z);

            var floatTarget = FloatHeight + Mathf.Sin((positionDelta.x + positionDelta.y)*PositionOffsetFrequency + Time.time * WaveFrequency) *
                WaveAmplitude;

            _rb.AddForce(new Vector3(0, floatTarget- _rb.position.y,0)*FloatForceStrength, ForceMode.Force);
            //_rb.position = new Vector3(_rb.position.x, floatTarget, _rb.position.z);

            AddRoundingForceFromBlackHole(positionDelta);
        }

        private void AddRoundingForceFromBlackHole(Vector2 positionDelta)
        {
            var positionDeltaNormalized = positionDelta.normalized;
            var forceDirection = positionDeltaNormalized.Rotate(95);
            var roundingForce = new Vector3(forceDirection.x, 0, forceDirection.y) * RoundingForce;
            _rb.AddForce(roundingForce, ForceMode.Force);
            //_rb.AddForce(-new Vector3(positionDeltaNormalized.x,0,positionDeltaNormalized.y)*roundingForce.magnitude*PullingForceFactor, ForceMode.Force);

        }
    }
}
