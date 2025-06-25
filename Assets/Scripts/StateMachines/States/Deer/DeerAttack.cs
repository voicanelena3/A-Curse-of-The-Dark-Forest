using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class DeerAttack : IState
{
    private IStateMachine owner;

    private GameObject projectilePrefab;
    private Transform shootPoint;

    private float shootCooldown = 2f;
    private float lastShootTime;
    private float shootDistance = 10f;
    private float chaseSpeed = 10f;

    private IState followState;
    public IState FollowState { get { return followState; } set { followState = value; } }

    public DeerAttack(IStateMachine owner)
    {
        this.owner = owner;
    }

    public void EnterState()
    {
        projectilePrefab = (owner as DeerStateMachine).GetComponent<DeerCombat>().projectilePrefab;
        shootPoint = (owner as DeerStateMachine).GetComponent<DeerCombat>().shootPoint;
    }

    public void ExecuteState()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        var deerStateMachine = owner as DeerStateMachine;
        if (Mathf.Abs((deerStateMachine.transform.position - enemy.transform.position).magnitude) > shootDistance)
        {
            float step = chaseSpeed * Time.deltaTime; // Calculate movement step based on speed and frame rate
            deerStateMachine.transform.position = Vector3.MoveTowards(deerStateMachine.transform.position, enemy.transform.position, step);
            return;
        }

        if (Time.time > lastShootTime + shootCooldown)
        {
            ShootAtClosestEnemy();
            lastShootTime = Time.time;
        }
    }

    public void LeaveState() { }

    public void NotifyMachine()
    {
        owner.ChangeState(followState);
    }

    private void ShootAtClosestEnemy()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");

        if (enemy != null && enemy.activeInHierarchy)
        {
            GameObject gameObject1 = GameObject.Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            DeerProjectile proj = gameObject1.GetComponent<DeerProjectile>();
            proj.Initialize(enemy.transform.position);
        }
    }

    private void ActivateSpecialAbility()
    {
        Debug.Log("Deer used its special ability!");

        // Example: launch 3 fast projectiles in a burst
        for (int i = 0; i < 3; i++)
        {
            (owner as DeerStateMachine).Invoke(nameof(ShootAtClosestEnemy), i * 0.3f);
        }

        // You can add healing, shield, or visual FX here
    }
}
