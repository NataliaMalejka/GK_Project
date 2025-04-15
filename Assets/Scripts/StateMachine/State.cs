using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected T controller;

    public State(T controller)
    {
        this.controller = controller;
    }

    public virtual void EnrterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void LateUpdateState() { }
    public virtual void ExitState() { }
}

