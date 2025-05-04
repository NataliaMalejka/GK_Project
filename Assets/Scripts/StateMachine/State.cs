using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected T controller;
    protected StateMachine<T> stateMachine;
    public State(T controller, StateMachine<T> stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void LateUpdateState() { }
    public virtual void ExitState() { }
}

