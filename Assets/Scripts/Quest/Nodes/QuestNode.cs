using System;
using Ai;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
[Serializable] public class UnityBoolPredicate : UnityEvent<BoxValue<bool>> { }

namespace Quest.Nodes
{
    
    public class QuestNode : MonoBehaviour
    {
        void Awake()
        {
            node = new State();
            node.AddOnBegin(onBegin.Invoke);
            node.AddOnUpdate(onUpdate.Invoke);
            node.AddOnEnd(onEnd.Invoke);
            node.AddShallReturn(() =>
            {
                BoxValue<bool> b = new BoxValue<bool>(true);
                shallReturn.Invoke(b);
                return b.value;
            });
            
            if (autoRun)
                QuestManager.instance.stateMachine.AddExistingState(node);
        }

        public State node { get; private set; }
        public bool autoRun = false;
        [Space]
        public UnityEvent onBegin;
        public UnityEvent onEnd;
        public UnityEvent onUpdate;
        public UnityBoolPredicate shallReturn;
        
        #region UtilityFunctions
        

        public void Log(string txt)
        {
            Debug.Log(txt);
        }
        public void ScheduleQuest(QuestNode quest)
        {
            QuestManager.instance.stateMachine.AddExistingState(quest.node);
        }

        
        #endregion UtilityFunctions
    }
}