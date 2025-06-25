using UnityEngine;

public class EnemyStateMachine : MonoBehaviour, IStateMachine
{
    private IState currentState;

    public void ChangeState(IState state)
    {
        currentState.LeaveState();
        currentState = state;
        currentState.EnterState();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var idleState = new EnemyIdle(this);
        var attackState = new EnemyAttack(this);

        idleState.AttackState = attackState;
        attackState.IdleState = idleState;

        currentState = idleState;
        currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.ExecuteState();
    }
}
