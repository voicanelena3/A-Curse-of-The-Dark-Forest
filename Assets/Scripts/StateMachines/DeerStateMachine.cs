using UnityEngine;

public class DeerStateMachine : MonoBehaviour, IStateMachine
{
    private IState currentState;
    private DeerInput deerInput;

    public void ChangeState(IState state)
    {
        currentState.LeaveState();
        currentState = state;
        currentState.EnterState();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        deerInput = this.gameObject.GetComponent<DeerInput>();

        var stayState = new DeerStay(this);
        var attackState = new DeerAttack(this);
        var followState = new DeerFollow(this);

        stayState.FollowState = followState;
        attackState.FollowState = followState;
        followState.StayState = stayState;
        followState.AttackState = attackState;

        currentState = stayState;
        currentState.EnterState();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        deerInput.Listen(currentState);
        currentState.ExecuteState();
    }
}
