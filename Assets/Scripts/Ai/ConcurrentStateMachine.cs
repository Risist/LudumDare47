using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    public class ConcurrentStateMachine : StateSet
    {
        public void UpdateStates()
        {
            for (int i = _states.Count - 1; i >= 0; --i)
            {
                var it = _states[i];
                it.Update();
                if (it.ShallReturn())
                {
                    RemoveState(it);
                }
            }
        }

        public void RemoveState(State toRemove)
        {
            if (toRemove == null)
            {
                Debug.LogWarning("trying to remove null state");
                return;
            }

            toRemove.End();
            _states.Remove(toRemove);
        }

        public new T AddExistingState<T>(T state) where T : State, new()
        {
            _states.Add(state);
            state.Begin();
            return state;
        }

        public new State AddNewState()
        {
            return AddNewState<State>();
        }

        public new T AddNewState<T>() where T : State, new()
        {
            var state = new T();
            AddExistingState(state);
            return state;
        }

    }
}