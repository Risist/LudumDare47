using UnityEngine;



namespace Ai
{
    [CreateAssetMenu(fileName = "FishBehaviourPack", menuName = "Ris/Ai/BehaviourPack/Fish", order = 0)]
    public class FishBehaviourPack : BehaviourPack
    {
        protected override void DefineBehaviours_Impl()
        {
            var stateMachine = controller.stateMachine;
            var inputHolder = controller.GetComponentInParent<InputHolder>();
            var transform = controller.transform;
            var fishReferences = controller.GetComponent<FishNpc>();
            var enemyFilter = GetAllyFilter();
            var obstacleFilter = GetEnemyFilter();
            Timer tState = new Timer();

            //var stateIdle = stateMachine.AddNewStateAsCurrent();

            var stateAway = stateMachine.AddNewStateAsCurrent();
            var stateSideFlow = stateMachine.AddNewState();
            var stateSideCounter = stateMachine.AddNewState();
            var stateInto = stateMachine.AddNewState();

            stateAway
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddShallReturn(tState.IsReady)
                .SetUtility(1.5f)
                .AddOnUpdate(() =>
            {
                Vector2 away = (transform.position - fishReferences.center.position).To2D().normalized;
                Vector2 side = new Vector2(-away.y, away.x);

                inputHolder.positionInput = away + Random.insideUnitCircle * 4.25f;

                inputHolder.keys[0] = Random.value <= 0.1f;

                float tongueChance =
                    fishReferences.tongue.currentState == Tongue.EState.EMoveForward ||
                    fishReferences.tongue.currentState == Tongue.EState.EPull ? 0.25f : 0.01f;

                if (enemyFilter.GetTarget() != null)
                {
                    tongueChance += 0.025f;

                    Vector2 toTarget = (enemyFilter.GetTarget().position - transform.position).To2D().normalized;
                    inputHolder.directionInput = toTarget;
                }
                inputHolder.keys[1] = Random.value <= tongueChance;
            });

            stateSideFlow
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddShallReturn(tState.IsReady)
                .SetUtility(1.75f)
                .AddOnUpdate(() =>
            {
                Vector2 away = (transform.position - fishReferences.center.position).To2D().normalized;
                Vector2 side = new Vector2(-away.y, away.x);

                inputHolder.positionInput = -side + Random.insideUnitCircle * 1.25f;

                inputHolder.keys[0] = Random.value <= 0.05f;

                float tongueChance =
                    fishReferences.tongue.currentState == Tongue.EState.EMoveForward ||
                    fishReferences.tongue.currentState == Tongue.EState.EPull ? 0.25f : 0.01f;

                if (enemyFilter.GetTarget() != null)
                {
                    tongueChance += 0.025f;

                    Vector2 toTarget = (enemyFilter.GetTarget().position - transform.position).To2D().normalized;
                    inputHolder.directionInput = toTarget;
                }
                inputHolder.keys[1] = Random.value <= tongueChance;
            });
            stateSideCounter
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddShallReturn(tState.IsReady)
                .SetUtility(0.75f)
                .AddOnUpdate(() =>
                {
                    Vector2 away = (transform.position - fishReferences.center.position).To2D().normalized;
                    Vector2 side = new Vector2(-away.y, away.x);

                    inputHolder.positionInput = side + Random.insideUnitCircle * 1.25f;

                    inputHolder.keys[0] = Random.value <= 0.05f;

                    float tongueChance =
                        fishReferences.tongue.currentState == Tongue.EState.EMoveForward ||
                        fishReferences.tongue.currentState == Tongue.EState.EPull ? 0.25f : 0.01f;

                    if (enemyFilter.GetTarget() != null)
                    {
                        tongueChance += 0.025f;

                        Vector2 toTarget = (enemyFilter.GetTarget().position - transform.position).To2D().normalized;
                        inputHolder.directionInput = toTarget;
                    }
                    inputHolder.keys[1] = Random.value <= tongueChance;
                });
            stateInto
                .AddOnBegin(() => tState.RestartRandom(0.5f, 0.75f))
                .AddShallReturn(tState.IsReady)
                .SetUtility(0.5f)
                .AddOnUpdate(() =>
                {
                    Vector2 away = (transform.position - fishReferences.center.position).To2D().normalized;
                    Vector2 side = new Vector2(-away.y, away.x);

                    inputHolder.positionInput = -side + Random.insideUnitCircle * 1.25f;

                    inputHolder.keys[0] = Random.value <= 0.05f;

                    float tongueChance =
                        fishReferences.tongue.currentState == Tongue.EState.EMoveForward ||
                        fishReferences.tongue.currentState == Tongue.EState.EPull ? 0.25f : 0.01f;

                    if (enemyFilter.GetTarget() != null)
                    {
                        tongueChance += 0.025f;

                        Vector2 toTarget = (enemyFilter.GetTarget().position - transform.position).To2D().normalized;
                        inputHolder.directionInput = toTarget;
                    }
                    inputHolder.keys[1] = Random.value <= tongueChance;
                });

            /*stateIdle.AddOnUpdate(() =>
            {
                Vector2 away = (transform.position - fishReferences.center.position).To2D().normalized;
                Vector2 side = new Vector2(-away.y, away.x);

                inputHolder.positionInput = away + -side*2 + Random.insideUnitCircle *4.25f;

                inputHolder.keys[0] = Random.value <= 0.1f;
            });*/
        }
    }
}