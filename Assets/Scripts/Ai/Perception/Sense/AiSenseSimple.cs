using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ai;

/*public class FocusStore : FocusBase
{
    public FocusStore(Transform transform) : base(transform)
    {
    }

    public MemoryEvent lastEvent = new MemoryEvent();

    public override MemoryEvent GetTarget()
    {
        return lastEvent;
    }
}

public class AiSenseSimple : MonoBehaviour
{
    public string blackboardKey = "simpleTarget";
    public Transform target;

    BehaviourController behaviourController;
    public FocusStore focus;

    private void Awake()
    {
        behaviourController = GetComponent<BehaviourController>();
        var v = behaviourController.InitBlackboardValue<FocusBase>(blackboardKey, () => new FocusStore(transform));
        focus = (FocusStore)v.value;
    }

    // Update is called once per frame
    void Update()
    {
        MemoryEvent ev = focus.lastEvent;
        ev.velocity = focus.lastEvent != null ?
            (Vector2)transform.position - focus.GetTargetPosition() :
            Vector2.zero;
        ev.exactPosition = target.position;
        ev.forward = transform.up;

        ev.lifetimeTimer = new MinimalTimer();
        ev.lifetimeTimer.Restart();
    }
}*/
