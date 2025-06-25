using UnityEngine;

public class DeerStay : IState
{
    private IStateMachine owner;

    private IState followState;
    public IState FollowState { get { return followState; } set { followState = value; } }

    public DeerStay(IStateMachine owner)
    {
        this.owner = owner;
    }

    public void EnterState() { }

    public void ExecuteState() { }

    public void LeaveState() { }

    public void NotifyMachine()
    {
        owner.ChangeState(followState);
    }
}
