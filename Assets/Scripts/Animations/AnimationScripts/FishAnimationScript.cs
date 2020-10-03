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
        var movement = stateMachine.GetComponent<RigidbodyMovementRotationVelocity>();

        var idleState = stateMachine.AddNewStateAsCurrent("Idle");
        var tongueState = stateMachine.AddNewState("Tongue");

        idleState.AddTransition(tongueState, new AnimationBlendData(0.25f));

        idleState.AddUpdate((s) =>
        {
            stateMachine.animator.SetBool("atWalk", inputHolder.atMove);
        });

        Timer tGlide = new Timer(2.75f);
        Timer cdGlide = new Timer(0.5f);
        tongueState
            .AddCanEnter(() => inputHolder.keys[0])
            .AddCanEnter(() => cdGlide.IsReady())
            .AddOnEnd(()=> cdGlide.Restart())
            .AddOnBegin(tGlide.Restart)
            .AddUpdate((t) =>
            {
                inputHolder.rotationInput = inputHolder.directionInput;
                if (!inputHolder.keys[0])
                    AutoTransition(stateMachine, idleState, new AnimationBlendData(0.35f, 0.0f), 0.85f);
            })
            .AddOnEnd(() => inputHolder.rotationInput = Vector2.zero);
    }
}