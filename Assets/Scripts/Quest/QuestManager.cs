using System;
using System.Collections.Generic;
using Ai;
using UnityEngine;


namespace Quest
{
    // TODO try to create GameObject:Node visual editing tools
    public class QuestManager : MonoSingleton<QuestManager>
    {
        public readonly ConcurrentStateMachine stateMachine = new ConcurrentStateMachine();
        
        public void Update()
        {
            stateMachine.UpdateStates();
        }
        
    }

}