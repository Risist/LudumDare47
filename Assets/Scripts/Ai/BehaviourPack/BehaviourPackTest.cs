using UnityEngine;

namespace Ai
{
    [CreateAssetMenu(fileName = "TestBehaviourPack", menuName = "Ris/Ai/BehaviourPack/Test", order = 0)]
    public class BehaviourPackTest : BehaviourPack
    {
        protected override void DefineBehaviours_Impl()
        {
            var stateMachine = controller.stateMachine;
            var inputHolder = controller.GetComponentInParent<InputHolder>();
            var transform = controller.transform;
            var enemyFilter = GetEnemyFilter();
            Timer tState = new Timer();

            
            var stateIdle = stateMachine.AddNewStateAsCurrent();
            var stateLookAround = stateMachine.AddNewState();
            var stateLookAt = stateMachine.AddNewState();
            var stateMoveTo = stateMachine.AddNewState();

            stateIdle
                .AddOnBegin(() => tState.RestartRandom(0.2f, 1.5f))
                .AddOnBegin(inputHolder.ResetInput)
                //.AddOnBegin(() => Debug.Log("At idle"))
                //.AddShallReturn(tState.IsReady)
                .AddCanTransitionToSelf(() => !tState.IsReady())
                .AddShallReturn(() => true)
            ;
            
            LookAround lookAround = new LookAround(transform,
                new RangedFloat(float.PositiveInfinity),
                new RangedFloat(30, 50) );

            var seeker = controller.GetComponent<Pathfinding.Seeker>();
            var moveToDestination = new MoveToDestinationNavigation(seeker);

            stateLookAround
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddOnBegin(inputHolder.ResetInput)
                .AddOnBegin(lookAround.Begin)
                //.AddOnBegin(() => Debug.Log("At rotation"))
                .AddOnUpdate(() =>
                {
                    inputHolder.rotationInput = lookAround.UpdateRotationInput();
                })
                .AddShallReturn(tState.IsReady)
                //.AddShallReturn(() => tState.IsReady() )
            ;
            LookAt lookAt = new LookAt(transform, 0.5f);


            stateLookAt
                .AddCanEnter(() => enemyFilter.GetTarget() != null)
                .SetUtility(() => 100)
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddOnBegin(inputHolder.ResetInput)
                .AddOnBegin(() => lookAt.SetDestination(enemyFilter.GetTarget()) )
                .AddOnUpdate(() =>
                {
                    lookAt.SetDestination(enemyFilter.GetTarget());
                    inputHolder.rotationInput = lookAt.UpdateRotationInput();
                })
                .AddShallReturn(() => enemyFilter.GetTarget() == null)
            ;

            stateMoveTo
                .AddCanEnter(() => false)
                .AddCanEnter(() => enemyFilter.GetTarget() != null)
                .SetUtility(() => 10000)
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddOnBegin(inputHolder.ResetInput)
                .AddOnBegin(() => moveToDestination.SetDestination(enemyFilter.GetTarget().position) )
                .AddOnUpdate(() =>
                {
                    moveToDestination.RepathAsNeeded(enemyFilter.GetTarget().position, 0.75f);
                    inputHolder.positionInput = moveToDestination.ToDestination(3.5f);
                })
                .AddShallReturn(() => enemyFilter.GetTarget() == null)
            ;


        }
    }
}