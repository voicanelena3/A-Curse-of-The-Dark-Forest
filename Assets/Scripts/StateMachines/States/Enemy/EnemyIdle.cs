using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyIdle : IState
{
    private IStateMachine owner;

    private float attackDistance = 20f;

    private IState attackState;
    public IState AttackState { get { return attackState; } set { attackState = value; } }

    public EnemyIdle(IStateMachine owner)
    {
        this.owner = owner;
    }

    public void EnterState() { }

    public void ExecuteState()
    {
        var enemyStateMachine = owner as EnemyStateMachine;

        var player = GameObject.FindWithTag("Player");
        float minDistance = Mathf.Abs((enemyStateMachine.transform.position - player.transform.position).magnitude);

        List<GameObject> deers = GameObject.FindGameObjectsWithTag("Deer").ToList();
        foreach (GameObject deer in deers)
        {
            float currentDistance = Mathf.Abs((enemyStateMachine.transform.position - deer.transform.position).magnitude);
            minDistance = Mathf.Min(minDistance, currentDistance);
        }

        if (minDistance < attackDistance) { NotifyMachine(); }
    }

    public void LeaveState() { }

    public void NotifyMachine()
    {
        owner.ChangeState(attackState);
    }
}
