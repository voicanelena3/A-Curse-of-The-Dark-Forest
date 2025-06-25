using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttack : IState
{
    IStateMachine owner;

    private GameObject projectilePrefab;

    private float followDistance = 40f;
    private float shootDistance = 10;
    private float chaseSpeed = 5f;
    private float shootCooldown = 3f;

    private float lastShootTime = 0f;

    private Transform target;

    private IState idleState;
    public IState IdleState {  get { return idleState; } set { idleState = value; } }

    public EnemyAttack(IStateMachine owner)
    {
        this.owner = owner;
    }

    public void EnterState()
    {
        projectilePrefab = (owner as EnemyStateMachine).gameObject.GetComponent<CombatController>().projectilePrefab;
        FindTarget();
    }

    public void ExecuteState()
    {
        var enemyStateMachine = owner as EnemyStateMachine;
        if (Mathf.Abs((enemyStateMachine.transform.position - target.transform.position).magnitude) > shootDistance)
        {
            float step = chaseSpeed * Time.deltaTime; // Calculate movement step based on speed and frame rate
            enemyStateMachine.transform.position = Vector3.MoveTowards(enemyStateMachine.transform.position, target.transform.position, step);
        }
        else if (Time.time > lastShootTime + shootCooldown)
        {
            ShootAtClosestEnemy();
            lastShootTime = Time.time;
        }

        if (Mathf.Abs((enemyStateMachine.transform.position - target.transform.position).magnitude) > followDistance)
        {
            FindTarget();
        }

        if (Mathf.Abs((enemyStateMachine.transform.position - target.transform.position).magnitude) > followDistance)
        {
            NotifyMachine();
        }
    }

    public void LeaveState() { }

    public void NotifyMachine()
    {
        owner.ChangeState(idleState);
    }

    private void FindTarget()
    {
        var enemyStateMachine = owner as EnemyStateMachine;

        var player = GameObject.FindWithTag("Player");
        float minDistance = Mathf.Abs((enemyStateMachine.transform.position - player.transform.position).magnitude);
        GameObject currentTarget = player;

        List<GameObject> deers = GameObject.FindGameObjectsWithTag("Deer").ToList();
        foreach (GameObject deer in deers)
        {
            float currentDistance = Mathf.Abs((enemyStateMachine.transform.position - deer.transform.position).magnitude);
            minDistance = Mathf.Min(minDistance, currentDistance);
            if (minDistance == currentDistance) { currentTarget = deer; }
        }

        target = currentTarget.transform;
    }

    private void ShootAtClosestEnemy()
    {
        var enemyStateMachine = owner as EnemyStateMachine;

        if (target != null && target.gameObject.activeInHierarchy)
        {
            GameObject gameObject1 = GameObject.Instantiate(projectilePrefab, enemyStateMachine.transform.position, Quaternion.identity);
            EnemyProjectile proj = gameObject1.GetComponent<EnemyProjectile>();
            proj.Initialize(target.transform.position);
            Debug.Log("Enemy attacked the deer!");
        }
    }
}
