using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Effects
{
    public class BlackHoleSparksController : MonoBehaviour
    {
        [SerializeField] private SphereCollider _blackHoleSphere;
        [SerializeField] private ParticleSystem _blackHoleSparklesPrefab;
        private float _toInsideSphereCorrectiveFactor = 0.2f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CreateSparkles(other);
            }
        }
        private void CreateSparkles(Collider other)
        {
            var closestPointOnSphere =
                _blackHoleSphere.ClosestPointOnBounds(other.transform.position + new Vector3(0, 0.5f, 0));
            var outSphereDirection = (closestPointOnSphere - _blackHoleSphere.transform.position).normalized;
            var sphereStartPoint = closestPointOnSphere - outSphereDirection * _toInsideSphereCorrectiveFactor;

            GameObject.Instantiate(_blackHoleSparklesPrefab, sphereStartPoint, Quaternion.LookRotation(outSphereDirection));
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            { 
                CreateSparkles(other);
            }
        }
    }
}
