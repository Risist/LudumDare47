using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Gameplay
{
    public class ForceOnCollision : MonoBehaviour
    {
        public Vector3 force;
        public Vector3 forceIncrease;
        private void OnTriggerStay(Collider other)
        {
            if (!other.attachedRigidbody || other.isTrigger || other.gameObject.layer == LayerMask.NameToLayer("Debris"))
                return;

            other.attachedRigidbody.AddForce(force);

            force += forceIncrease;
        }
        private void OnCollisionStay(Collision other)
        {
            if (!other.rigidbody || other.collider.isTrigger || other.gameObject.layer == LayerMask.NameToLayer("Debris"))
                return;

            other.rigidbody.AddForce(force);

            force += forceIncrease;
        }
    }
}