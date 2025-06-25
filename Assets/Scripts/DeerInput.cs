using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DeerInput : MonoBehaviour
{
    private IState currentState;
    private Dictionary<Type, Action> listenDictionary;

    public GameObject followNotification;

    private void Start()
    {
        listenDictionary = new Dictionary<Type, Action>()
        {
            { typeof(DeerStay), StayState },
            { typeof(DeerFollow), FollowState },
            { typeof(DeerAttack), AttackState }
        };
    }

    public void Listen(IState deerState)
    {
        currentState = deerState;
        if (Input.GetKeyDown(KeyCode.M)) listenDictionary[deerState.GetType()]();
    }

    private void StayState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);
        if (!hasHit) { return; }

        var stateMachine = hit.transform.GetComponent<DeerStateMachine>();
        if (stateMachine == null) { return; }
        if (stateMachine.gameObject != this.gameObject) { return; }

        followNotification.SetActive(true);
        currentState.NotifyMachine();
    }

    private void FollowState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);
        if (!hasHit) { return; }

        var followState = currentState as DeerFollow;

        var stateMachine = hit.transform.GetComponent<DeerStateMachine>();
        if (stateMachine == null)
        {
            var combatController = hit.transform.GetComponent<CombatController>();
            if (combatController == null) { return; }

            followState.NextState = followState.AttackState;
            currentState.NotifyMachine();
            return;
        }

        followState.NextState = followState.StayState;
        followNotification.SetActive(false);
        currentState.NotifyMachine();
    }

    private void AttackState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);
        if (!hasHit) { return; }

        var stateMachine = hit.transform.GetComponent<DeerStateMachine>();
        if (stateMachine == null) { return; }
        if (stateMachine.gameObject != this.gameObject) { return; }

        currentState.NotifyMachine();
    }
}
