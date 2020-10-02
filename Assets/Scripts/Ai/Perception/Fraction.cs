using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ai
{
    [CreateAssetMenu(fileName = "Fraction", menuName = "Ris/Ai/Fraction")]
    public class Fraction : ScriptableObject
    {
        public enum EAttitude
        {
            EFriendly,
            ENeutral,
            EEnemy,
            ENone
        }

        public Color fractionColorUi;
        public Fraction[] friendlyFractions;
        public Fraction[] enemyFractions;

        public EAttitude GetAttitude(Fraction fraction)
        {
            if (Equals(fraction))
                return EAttitude.EFriendly;

            foreach (var it in friendlyFractions)
                if (it.Equals(fraction))
                    return EAttitude.EFriendly;

            foreach (var it in enemyFractions)
                if (it.Equals(fraction))
                    return EAttitude.EEnemy;

            return EAttitude.ENeutral;
        }
    }
}
