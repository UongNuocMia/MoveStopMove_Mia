using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{ 
    private IState<Bot> currentState;

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
            currentState.OnExecute(this);
    }
    public void Move(Transform target)
    {
        if (!agent.isOnNavMesh) return;
        Vector3 moveDirection = new Vector3(target.position.x, target.position.y, target.position.z);
        Vector3 destination = moveDirection; 
        agent.speed = speed;
        agent.SetDestination(destination);
        TF.forward = Vector3.Slerp(TF.forward, moveDirection, 0);
        ChangeAnim(Constants.RUN_ANIM);
    }
    public void ChangeState(IState<Bot> newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
    public override void OnStartGame()
    {
        base.OnStartGame();
        ChangeState(new PatrolState());
    }
    public override void OnEndGame()
    {
        base.OnEndGame();
        Move(transform);
        ChangeState(new IdleState());
    }

    public override void OnPrepareGame()
    {
        base.OnPrepareGame();
        ChangeState(new IdleState());
    }
    public override void OnSetting()
    {
        Move(transform);
        ChangeState(new IdleState());
    }
}
