using System.Collections.Generic;
using System;
using UnityEngine;

namespace Ai
{
    public class AttentionPicker : MonoBehaviour
    {
        List<AttentionMode> _attentionModes = new List<AttentionMode>();
        public AttentionMode currentMode { get; private set; }
        public float currentModeBias { get; private set; }

        #region Initialization
        public AttentionPicker SetCurrentModeBias(float s)
        {
            currentModeBias = s;
            return this;
        }
        public AttentionMode CreateNewAttentionMode()
        {
            var mode = new AttentionMode();
            _attentionModes.Add(mode);
            return mode;
        }
        
        #endregion Initialization

        AttentionMode GetBestMode()
        {
            float bestUtility = currentModeBias;
            if (currentMode != null)
                bestUtility += currentMode.GetUtility();
            AttentionMode bestSource = currentMode;

            int n = _attentionModes.Count;
            for (int i = 0; i < n; ++i)
            {
                var source = _attentionModes[i];
                float utility = source.GetUtility();
                if(utility > bestUtility)
                {
                    bestSource = _attentionModes[i];
                }
            }
            return bestSource;
        }
        void Update()
        {
            currentMode = GetBestMode();
        }

        #region NextStateMethods

        public State GetRandomTransition()
        {
            return currentMode?.GetRandomTransition();
        }

        public State GetTransitionByRandomUtility()
        {
            return currentMode?.GetTransitionByRandomUtility();
        }

        public State GetTransitionByBestUtility()
        {
            return currentMode?.GetTransitionByBestUtility();
        }

        public State GetTransitionByOrder()
        {
            return currentMode?.GetTransitionByOrder();
        }

        #endregion NextStateMethods
    }


    public class AttentionMode : StateSet
    {
        Func<float> _getUtility = () => 0.0f;

        public AttentionMode SetUtility(float s)
        {
            _getUtility = () => s;
            return this;
        }
        public AttentionMode SetUtility(Func<float> s)
        {
            _getUtility = s;
            return this;
        }

        public float GetUtility()
        {
            return _getUtility();
        }
    }
}
