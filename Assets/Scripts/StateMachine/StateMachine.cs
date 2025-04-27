using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    public State<T> currentState;

    public void InitializeState(State<T> startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void UpdateCurrentState()
    {
        currentState.UpdateState();
    } 

    public void FixedUpdateCurrentState()
    {
        currentState.FixedUpdateState();
    }

    public void LateUpdateCurrentState()
    {
        currentState.LateUpdateState();
    }

    public void ChangeState(State<T> newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
