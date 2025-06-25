using UnityEngine;

public interface IState
{
    void EnterState();
    void ExecuteState();
    void LeaveState();
    void NotifyMachine();
}

public interface IStateMachine
{
    void ChangeState(IState state);
}
