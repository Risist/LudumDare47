using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    public abstract class BehaviourPack : ScriptableObject
    {
        public void DefineBehaviours(BehaviourController controller)
        {
            this.controller = controller;
            DefineBehaviours_Impl();
        }
        protected abstract void DefineBehaviours_Impl();
        
        protected BehaviourController controller;
        protected AttentionPicker attentionPicker => controller.attentionPicker;
        
        ////////// Utility
        ///

        public const string enemyId = nameof(enemyId);
        public const string allyId = nameof(allyId);
        public const string neutralId = nameof(neutralId);
        public const string noiseId = nameof(noiseId);
        public const string painId = nameof(painId);
        public const string touchId = nameof(touchId);

        protected StimuliFilter GetEnemyFilter()
        {
            var storage = controller.GetBlackboardValue<StimuliStorage>(enemyId).value;
            return controller.InitBlackboardValue<StimuliFilter>(enemyId, () => new StimuliFilter(storage, controller.transform)).value;
        }
        protected StimuliFilter GetAllyFilter()
        {
            var storage = controller.GetBlackboardValue<StimuliStorage>(allyId).value;
            return controller.InitBlackboardValue<StimuliFilter>(allyId, () => new StimuliFilter(storage, controller.transform)).value;
        }
        protected StimuliFilter GetNeutralFilter()
        {
            var storage = controller.GetBlackboardValue<StimuliStorage>(neutralId).value;
            return controller.InitBlackboardValue<StimuliFilter>(neutralId, () => new StimuliFilter(storage, controller.transform)).value;
        }
        protected StimuliFilter GetNoiseFilter()
        {
            var storage = controller.GetBlackboardValue<StimuliStorage>(noiseId).value;
            return controller.InitBlackboardValue<StimuliFilter>(noiseId, () => new StimuliFilter(storage, controller.transform)).value;
        }
        protected StimuliFilter GetPainFilter()
        {
            var storage = controller.GetBlackboardValue<StimuliStorage>(painId).value;
            return controller.InitBlackboardValue<StimuliFilter>(painId, () => new StimuliFilter(storage, controller.transform)).value;
        }
        protected StimuliFilter GetTouchFilter()
        {
            var storage = controller.GetBlackboardValue<StimuliStorage>(touchId).value;
            return controller.InitBlackboardValue<StimuliFilter>(touchId, () => new StimuliFilter(storage, controller.transform)).value;
        }

        protected AttentionMode GetEnemyMode()
        {
            return controller.InitBlackboardValue(enemyId, () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetAllyMode()
        {
            return controller.InitBlackboardValue(allyId, () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetNeutralMode()
        {
            return controller.InitBlackboardValue(neutralId, () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetNoiseMode()
        {
            return controller.InitBlackboardValue(noiseId, () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetPainMode()
        {
            return controller.InitBlackboardValue(painId, () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetTouchMode()
        {
            return controller.InitBlackboardValue(touchId, () => attentionPicker.CreateNewAttentionMode()).value;
        }

        protected AttentionMode GetEnemyShadeMode()
        {
            return controller.InitBlackboardValue(enemyId + "Shade", () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetAllyShadeMode()
        {
            return controller.InitBlackboardValue(allyId + "Shade", () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetNeutralShadeMode()
        {
            return controller.InitBlackboardValue(neutralId + "Shade", () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetNoiseShadeMode()
        {
            return controller.InitBlackboardValue(noiseId + "Shade", () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetPainShadeMode()
        {
            return controller.InitBlackboardValue(painId + "Shade", () => attentionPicker.CreateNewAttentionMode()).value;
        }
        protected AttentionMode GetTouchShadeMode()
        {
            return controller.InitBlackboardValue(touchId + "Shade", () => attentionPicker.CreateNewAttentionMode()).value;
        }
    }
}
