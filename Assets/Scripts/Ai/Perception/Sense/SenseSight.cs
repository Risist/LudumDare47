using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
/*
 *
 * OverlapCircle check by memorableMask
 * to all hit targets which have AiPerceiveUnit and are in given cone cast ray
 * The ray will check if there is no obstacle at the way from perceiver to target
 *
 * All targets that pass the test will be pushed into special AiFocus type
 *      that will store targets and sort them when needed
 *
 */
    [RequireComponent(typeof(BehaviourController))]
    public class SenseSight : SenseBase
    {
        [Header("VisionSettings")] public LayerMask memorableMask;
        public LayerMask obstacleMask;
        public bool trackEnemy = true;
        public bool trackAlly = false;
        public bool trackNeutrals = false;


        [Header("Shape")] public float coneAngle = 170.0f;
        public float searchDistance = 5.0f;

        protected Transform cachedTransform;

        public StimuliStorage enemyStorage { get; protected set; }
        public StimuliStorage allyStorage { get; protected set; }
        public StimuliStorage neutralStorage { get; protected set; }

        void InsertEvent(MemoryEvent ev, Fraction.EAttitude attitude)
        {
            switch (attitude)
            {
                case Fraction.EAttitude.EEnemy:
                    if (trackEnemy)
                        enemyStorage.PerceiveEvent(ev);
                    break;
                case Fraction.EAttitude.EFriendly:
                    if (trackAlly)
                        allyStorage.PerceiveEvent(ev);
                    break;
                case Fraction.EAttitude.ENeutral:
                    if (trackNeutrals)
                        neutralStorage.PerceiveEvent(ev);
                    break;
            }
        }

        new void Awake()
        {
            base.Awake();

            // cache transform
            cachedTransform = base.transform;

            enemyStorage = RegisterSenseInBlackboard(BehaviourPack.enemyId);
            allyStorage = RegisterSenseInBlackboard(BehaviourPack.allyId);
            neutralStorage = RegisterSenseInBlackboard(BehaviourPack.neutralId);
        }

        #region FocusManager

        static readonly List<SenseSight> _sightList = new List<SenseSight>();
        static float _searchTime;

        protected void OnEnable() => _sightList.Add(this);

        protected void OnDisable() => _sightList.Remove(this);

        // in order for automatic search to occur for 3d colliders this coroutine has to be called 
        public static IEnumerator PerformSearch_Coroutine(float searchTime)
        {
            _searchTime = searchTime;
            var wait = new WaitForSeconds(searchTime);
            while (true)
            {
                yield return wait;

                foreach (var it in _sightList)
                    it.PerformSearch();
            }
        }

        // in order for automatic search to occur for 2d colliders this coroutine has to be called
        public static IEnumerator PerformSearch2D_Coroutine(float searchTime)
        {
            _searchTime = searchTime;
            var wait = new WaitForSeconds(searchTime);
            while (true)
            {
                yield return wait;

                foreach (var it in _sightList)
                    it.PerformSearch2D();
            }
        }

        #endregion FocusManager

        #region Search

        void PerformSearch2D()
        {
            Fraction myFraction = myUnit.fraction;
            if (!myFraction)
            {
#if UNITY_EDITOR
                Debug.LogWarning("No fraction in perceive unit but trying to use sight");
#endif
                // there's no way to determine where to put events
                // do not even bother
                return;
            }

            // perform cast
            int n = Physics2D.OverlapCircleNonAlloc(cachedTransform.position, searchDistance,
                StaticCacheLists.colliderCache2D, memorableMask);

            // preselect targets
            // they have to be in proper angle and contain PerceiveUnit
            for (int i = 0; i < n; ++i)
            {
                var it = StaticCacheLists.colliderCache2D[i];
                Transform itTransform = it.transform;

                //// check if the target is in proper angle
                Vector2 toIt = itTransform.position - cachedTransform.position;
                float cosAngle = Vector2.Dot(toIt.normalized, cachedTransform.up);
                float angle = Mathf.Acos(cosAngle) * 180 / Mathf.PI;
                //Debug.Log(angle);
                bool bProperAngle = angle < coneAngle * 0.5f;
                if (!bProperAngle)
                    continue;

                // ok, now check if it has AiPerceiveUnit
                // we need it's fraction to determine our attitude

                PerceiveUnit perceiveUnit = it.GetComponent<PerceiveUnit>();
                if (perceiveUnit == myUnit)
                    // oh, come on do not look at yourself... don't be soo narcissistic
                    continue;

                if (!perceiveUnit)
                    // no perceive unit, this target is invisible to us
                    continue;

                Fraction itFraction = perceiveUnit.fraction;
                if (!itFraction)
                    // the same as above,
                    return;

                //// determine attitude
                Fraction.EAttitude attitude = myFraction.GetAttitude(itFraction);

                //// Check if obstacles blocks vision
                foreach(var itObstacleTest in perceiveUnit.obstacleTestTargets)
                    if (DoObstaclesBlockVision2D(itObstacleTest.position))
                        continue;

                //// create event
                var rb = it.attachedRigidbody;
                MemoryEvent ev = new MemoryEvent
                {
                    exactPosition = itTransform.position,
                    forward = itTransform.up,
                    
                    // if collider has rigidbody then take its velocity
                    // otherwise there is no simple way to determine event velocity
                    velocity = rb ? rb.velocity * velocityPredictionScale : Vector2.zero,
                  
                    // set up agent reponsible for this event
                    perceiveUnit = perceiveUnit
                };
                // ensure event will tick from now on
                ev.lifetimeTimer.Restart();


                Debug.DrawRay(ev.exactPosition, Vector3.up, Color.blue, _searchTime * nEvents);
                Debug.DrawRay(ev.exactPosition, ev.velocity * _searchTime, Color.gray, _searchTime);
                InsertEvent(ev, attitude);
            }
        }

        bool DoObstaclesBlockVision2D(Vector2 target)
        {
            // we will change searchDistance based on visibility of obstacles;
            float localSearchDistance = this.searchDistance;

            Vector2 toTarget = target - (Vector2) cachedTransform.position;
            float toTargetSq = toTarget.sqrMagnitude;


            int n = Physics2D.RaycastNonAlloc(cachedTransform.position, toTarget, StaticCacheLists.raycastHitCache2D,
                toTarget.magnitude, obstacleMask);

            bool bObstaclesBlocksVision = false;
            for (int i = 0; i < n; ++i)
            {
                var it = StaticCacheLists.raycastHitCache2D[i];
                PerceiveUnit unit = it.collider.GetComponent<PerceiveUnit>();
                if (!unit)
                {
                    // we assume objects that do not have perceive unit will behave as non transparent
                    // so we can't see our target
                    bObstaclesBlocksVision = true;
                    break;
                }

                if (unit == myUnit)
                    // well, i'm not that fat ... i guess
                    continue;


                localSearchDistance *= unit.transparencyLevel;
                if (localSearchDistance * localSearchDistance < toTargetSq * myUnit.distanceModificator)
                    // transparency is reduced too much to see the target
                {
                    bObstaclesBlocksVision = true;
                    break;
                }
            }

            Debug.DrawRay(cachedTransform.position, toTarget, bObstaclesBlocksVision ? Color.yellow : Color.green, 0.25f);


            return bObstaclesBlocksVision;
        }

        void PerformSearch()
        {
            Fraction myFraction = myUnit.fraction;

            if (!myFraction)
            {
#if UNITY_EDITOR
                Debug.LogWarning("No fraction in perceive unit but trying to use sight");
#endif
                // there's no way to determine where to put events
                return;
            }

            // perform cast
            int n = Physics.OverlapSphereNonAlloc(cachedTransform.position, searchDistance, StaticCacheLists.colliderCache,
                memorableMask);

            // preselect targets
            // they have to be in proper angle and contain PerceiveUnit
            for (int i = 0; i < n; ++i)
            {
                var it = StaticCacheLists.colliderCache[i];
                Transform itTransform = it.transform;

                //// check if the target is in proper angle
                Vector3 toIt = itTransform.position - cachedTransform.position;
                toIt = toIt.To2D();

                float cosAngle = Vector2.Dot(toIt.normalized, cachedTransform.forward.To2D());
                float angle = Mathf.Acos(cosAngle) * 180 / Mathf.PI;
                //Debug.Log(angle);
                bool bProperAngle = angle < coneAngle * 0.5f;
                if (!bProperAngle)
                    continue;

                // ok, now check if it has PerceiveUnit component
                // we need it's fraction to determine our attitude

                PerceiveUnit perceiveUnit = it.GetComponent<PerceiveUnit>();
                if (perceiveUnit == myUnit)
                    // oh, come on do not look at yourself... don't be soo narcissistic
                    continue;

                if (!perceiveUnit)
                    // no perceive unit, this target is invisible to us
                    continue;

                Fraction itFraction = perceiveUnit.fraction;
                if (!itFraction)
                    // the same as above,
                    return;

                //// determine attitude
                Fraction.EAttitude attitude = myFraction.GetAttitude(itFraction);

                //// Check if obstacles blocks vision
                if (DoObstaclesBlockVision(itTransform.position))
                    continue;

                //// create event
                var rb = it.attachedRigidbody;
                MemoryEvent ev = new MemoryEvent
                {
                    exactPosition = itTransform.position,
                    forward = itTransform.up,

                    // if collider has rigidbody then take its velocity
                    // otherwise there is no simple way to determine event velocity
                    velocity = rb ? rb.velocity * velocityPredictionScale : Vector3.zero,
                    // set up agent responsible for this event
                    perceiveUnit = perceiveUnit
                };


                // ensure event will tick from now on
                ev.lifetimeTimer.Restart();


                Debug.DrawRay(itTransform.position, Vector3.up, Color.blue, _searchTime * nEvents);
                Debug.DrawRay(itTransform.position, ev.velocity * _searchTime, Color.gray, _searchTime);
                InsertEvent(ev, attitude);
            }
        }

        bool DoObstaclesBlockVision(Vector3 target)
        {
            // we will change searchDistance based on visibility of obstacles;
            float localSearchDistance = this.searchDistance;

            Vector3 toTarget = target - cachedTransform.position;
            float toTargetSq = toTarget.ToPlane().sqrMagnitude;


            int n = Physics.RaycastNonAlloc(cachedTransform.position, toTarget, StaticCacheLists.raycastHitCache,
                toTarget.magnitude, obstacleMask);

            bool bObstaclesBlocksVision = false;
            for (int i = 0; i < n; ++i)
            {
                var it = StaticCacheLists.raycastHitCache[i];

                PerceiveUnit unit = it.collider.GetComponent<PerceiveUnit>();
                if (!unit)
                {
                    // we assume objects that do not have perceive unit will behave as non transparent
                    // so we can't see our target
                    bObstaclesBlocksVision = true;
                    break;
                }

                if (unit == myUnit)
                    // well, i'm not that fat ... i guess
                    continue;


                localSearchDistance *= unit.transparencyLevel;
                if (localSearchDistance * localSearchDistance < toTargetSq * myUnit.distanceModificator)
                    // transparency is reduced too much to see the target
                {
                    bObstaclesBlocksVision = true;
                    break;
                }
            }

            Debug.DrawRay(cachedTransform.position, toTarget, bObstaclesBlocksVision ? Color.yellow : Color.green, 0.25f);


            return bObstaclesBlocksVision;
        }


        #endregion Search


        void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.matrix = transform.localToWorldMatrix;

            Vector3 start = Vector3.zero;
            Vector3 end = start + Quaternion.Euler(0, -coneAngle * 0.5f, 0) * Vector3.forward * searchDistance;
            UnityEditor.Handles.DrawLine(start, end);

            end = start + Quaternion.Euler(0, coneAngle * 0.5f, 0) * Vector3.forward * searchDistance;
            UnityEditor.Handles.DrawLine(start, end);


            end = start + Vector3.forward * searchDistance;
            UnityEditor.Handles.DrawLine(start, end);


            UnityEditor.Handles.DrawWireDisc(start, transform.up, searchDistance);
#endif
        }
    }
}


    