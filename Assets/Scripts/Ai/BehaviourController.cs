using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai;

namespace Ai
{
    public class BehaviourController : MonoBehaviour
    {
        #region References

        [NonSerialized] public readonly StateMachine stateMachine = new StateMachine();
                        public AttentionPicker attentionPicker { get; private set; }

        #endregion References


        #region Blackboard

        readonly GenericBlackboard _blackboard = new GenericBlackboard();

        public BoxValue<T> InitBlackboardValue<T>(string key, Func<T> initializeMethod)
        {
            return _blackboard.InitValue<T>(key, initializeMethod);
        }

        public BoxValue<T> InitBlackboardValue<T>(string key) where T : new()
        {
            return _blackboard.InitValue<T>(key);
        }

        public BoxValue<T> GetBlackboardValue<T>(string key)
        {
            return _blackboard.GetValue<T>(key);
        }

        #endregion Blackboard

        #region BehaviourPack

        [Tooltip("Behaviour packs are used to insert groups of behaviours to this agent")] [SerializeField]
        BehaviourPack[] _behaviourPacks = new BehaviourPack[0];

        void InitBehaviourPacks()
        {
            foreach (var it in _behaviourPacks)
                it.DefineBehaviours(this);
        }

        #endregion BehaviourPack

        private void Start()
        {
            InitBehaviourPacks();
            Debug.Assert(stateMachine.currentState != null, "No behaviour pack initialized current state");
            attentionPicker = GetComponent<AttentionPicker>();
        }

        private void FixedUpdate()
        {
            stateMachine.UpdateStates();
        }
    }

}
