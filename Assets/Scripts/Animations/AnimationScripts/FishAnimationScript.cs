using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FishAnimationScript", menuName = "Ris/Anim/fishAnimationScript", order = 0)]
public class FishAnimationScript : AnimationScript
{
    public override void InitAnimation(AnimationStateMachine stateMachine)
    {
        var inputHolder = stateMachine.GetComponent<InputHolder>();

        var idleState = stateMachine.AddNewStateAsCurrent("Idle");
        idleState.AddUpdate((s) =>
        {
            stateMachine.animator.SetBool("atWalk", inputHolder.atMove);
        });
    }
}