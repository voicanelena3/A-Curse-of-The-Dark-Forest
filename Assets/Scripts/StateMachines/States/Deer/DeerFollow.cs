using UnityEngine;

public class DeerFollow : IState
{
    private IStateMachine owner;

    private Transform ownerTransform;
    private Transform playerTransform; // Reference to the Player's Transform
    private float followSpeed = 5f; // Speed at which the Deer moves

    private IState stayState;
    public IState StayState { get { return stayState; } set { stayState = value; } }
    
    private IState attackState;
    public IState AttackState { get { return attackState; } set { attackState = value; } }

    private IState nextState;
    public IState NextState { get { return nextState; } set { nextState = value; } }

    public DeerFollow(IStateMachine owner)
    {
        this.owner = owner;
        ownerTransform = (owner as DeerStateMachine).transform;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void EnterState() { Debug.Log("Deer started following the player."); }

    public void ExecuteState()
    {
        // Move the Deer towards the Player
        float step = followSpeed * Time.deltaTime; // Calculate movement step based on speed and frame rate
        ownerTransform.position = Vector3.MoveTowards(ownerTransform.position, playerTransform.position, step);
    }

    public void LeaveState() { Debug.Log("Deer stopped following the player."); }

    public void NotifyMachine()
    {
        owner.ChangeState(nextState);
    }
}
