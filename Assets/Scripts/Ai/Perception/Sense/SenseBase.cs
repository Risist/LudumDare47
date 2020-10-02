using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    /*public class FocusFilterStorage
    {
        public FocusFilterStorage( StimuliStorage storage,
            Func<MemoryEvent, float> measureMethod)
        {
            Debug.Assert(storage != null);
            Debug.Assert(measureMethod != null);

            this.storage = storage;
            this.measureMethod = measureMethod;

        }

        // filters by distance from center
        public FocusFilterStorage(Transform transform, StimuliStorage storage, 
            float distanceUtilityScale = 0.0f, float timeUtilityScale = 1.0f, float maxLifetime = float.MaxValue )
        {
            Debug.Assert(storage != null);

            this.storage = storage;
            this.measureMethod = (MemoryEvent memoryEvent) =>
            {
                if (memoryEvent.elapsedDistance > maxLifetime)
                    return float.MinValue;

                float timeUtility = -memoryEvent.elapsedTime;
                float distanceUtility = -(transform.position - memoryEvent.position).ToPlane().magnitude;

                float utility = timeUtility * timeUtilityScale + distanceUtility * distanceUtilityScale;
                return utility;
            };
        }

        // filters by distance from local position
        public FocusFilterStorage(Transform transform, StimuliStorage storage,
            Vector2 localPoint, float distanceUtilityScale = 0.0f, float timeUtilityScale = 1.0f, float maxLifetime = float.MaxValue)
        {
#if UNITY_EDITOR
            Debug.Assert(storage != null);
#endif

            this.storage = storage;
            this.measureMethod = (MemoryEvent memoryEvent) =>
            {
                if (memoryEvent.elapsedDistance > maxLifetime)
                    return float.MaxValue;

                float timeUtility = -memoryEvent.elapsedTime;
                float distanceUtility = -(transform.TransformPoint(localPoint) - memoryEvent.position).ToPlane().magnitude;

                float utility = timeUtility * timeUtilityScale + distanceUtility * distanceUtilityScale;
                return utility;
            };
        }


        protected Func<MemoryEvent, float> measureMethod;
        protected StimuliStorage storage;

        // event cache mechanism
        // used to ensure only one memory search will be performed
        // Time.time stays constant throughout frame so we can check if the value has changed since
        // if soo we update stored event
        protected void UpdateEvent()
        {
            if (Time.time != lastFrame)
            {
                lastFrame = Time.time;
                lastEvent = storage.FindBestEvent(measureMethod);
                if(lastEvent != null)
                    Debug.DrawLine(lastEvent.exactPosition, lastEvent.position, Color.yellow, Time.fixedDeltaTime, false);
            }
        }
        float lastFrame = -1;
        MemoryEvent lastEvent;

        public MemoryEvent GetTarget()
        {
            UpdateEvent();
            return lastEvent;
        }
    }*/

    public class StimuliFilter
    {
        public StimuliFilter(StimuliStorage storage,
               Func<MemoryEvent, float> measureMethod)
        {
            Debug.Assert(storage != null);
            Debug.Assert(measureMethod != null);

            this.storage = storage;
            this.measureMethod = measureMethod;

        }
        // filters by distance from center
        public StimuliFilter(StimuliStorage storage, Transform transform,
            float distanceUtilityScale = 0.0f, float timeUtilityScale = 1.0f, float maxLifetime = float.MaxValue)
        {
            Debug.Assert(storage != null);

            this.storage = storage;
            this.measureMethod = (MemoryEvent memoryEvent) =>
            {
                if (memoryEvent.elapsedDistance > maxLifetime)
                    return float.MinValue;

                float timeUtility = -memoryEvent.elapsedTime;
                float distanceUtility = -(transform.position - memoryEvent.position).ToPlane().magnitude;

                float utility = timeUtility * timeUtilityScale + distanceUtility * distanceUtilityScale;
                return utility;
            };
        }
        // filters by distance from local position
        public StimuliFilter( StimuliStorage storage, Transform transform,
            Vector2 localPoint, float distanceUtilityScale = 0.0f, float timeUtilityScale = 1.0f, float maxLifetime = float.MaxValue)
        {
            Debug.Assert(storage != null);

            this.storage = storage;
            this.measureMethod = (MemoryEvent memoryEvent) =>
            {
                if (memoryEvent.elapsedDistance > maxLifetime)
                    return float.MaxValue;

                float timeUtility = -memoryEvent.elapsedTime;
                float distanceUtility = -(transform.TransformPoint(localPoint) - memoryEvent.position).ToPlane().magnitude;

                float utility = timeUtility * timeUtilityScale + distanceUtility * distanceUtilityScale;
                return utility;
            };
        }

        StimuliStorage storage;
        Func<MemoryEvent, float> measureMethod;

        // event cache mechanism
        // used to ensure only one memory search will be performed
        // Time.time stays constant throughout frame so we can check if the value has changed since
        // if soo we update stored event
        protected void UpdateEvent()
        {
            if (Time.time != lastFrame)
            {
                lastFrame = Time.time;
                lastEvent = storage.FindBestEvent(measureMethod);
                if (lastEvent != null)
                    Debug.DrawLine(lastEvent.exactPosition, lastEvent.position, Color.yellow, Time.fixedDeltaTime, false);
            }
        }
        float lastFrame = -1;
        MemoryEvent lastEvent;

        public MemoryEvent GetTarget()
        {
            UpdateEvent();
            return lastEvent;
        }
    }

    // Stores stimuli collected by given senses
    // behaviours can then read from it directly or through focus
    public class StimuliStorage
    {
        public StimuliStorage(int nEvents, float maxEventLifetime)
        {
            memoryEvents = new MemoryEvent[nEvents];
            this.maxEventLifetime = maxEventLifetime;
        }

        float maxEventLifetime;
        MemoryEvent[] memoryEvents;
        int lastAddedEvent = -1;

        // pushes event onto perceived events list
        // restarts lifetimeTimer
        public void PerceiveEvent(MemoryEvent newEvent)
        {
#if UNITY_EDITOR
            if (newEvent == null)
            {
                Debug.LogWarning("Trying to register null as MemoryEvent");
                return;
            }
#endif
            // check if in memory there is any event caused by the same unit as newEvent
            if (newEvent.perceiveUnit)
            {
                for (int i = 0; i < memoryEvents.Length; ++i)
                    if (memoryEvents[i] != null && memoryEvents[i].perceiveUnit == newEvent.perceiveUnit)
                    {
                        memoryEvents[i] = null;
                        break;
                    }
            }

            lastAddedEvent = (lastAddedEvent + 1) % memoryEvents.Length;
            memoryEvents[lastAddedEvent] = newEvent;
            newEvent.lifetimeTimer.Restart();
        }

        // returns best event from registered ones by measureEventMethod
        // the one with greatest evaluation value will be returned or null if none is valid
        // Events are considered as valid if they are not null and they are not older than maxEventLifeTime
        public MemoryEvent FindBestEvent(System.Func<MemoryEvent, float> measureEventMethod)
        {
            MemoryEvent bestEvent = null;
            float bestUtility = float.MinValue;

            foreach (var it in memoryEvents)
            {
                if (it == null || it.elapsedTime > maxEventLifetime)
                    continue;


                float utility = measureEventMethod(it);
                if (utility > bestUtility)
                {
                    bestUtility = utility;
                    bestEvent = it;
                }
            }

            return bestEvent;
        }
    }


    public abstract class SenseBase : MonoBehaviour
    {
        [Header("General")] public int nEvents;
        public float maxEventLifeTime;
        public float velocityPredictionScale = 1.0f;


        protected PerceiveUnit myUnit;
        protected BehaviourController behaviourController;


        protected void Awake()
        {
            myUnit = GetComponentInParent<PerceiveUnit>();
            behaviourController = GetComponentInChildren<BehaviourController>();
        }

        // registers event
        protected StimuliStorage RegisterSenseInBlackboard(string blackboardName)
        {
            Debug.Assert(behaviourController);
            var storage = behaviourController.InitBlackboardValue(blackboardName,
                () => new StimuliStorage(nEvents, maxEventLifeTime));
            return storage.value;
        }

    }
}