using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{ 
    [SerializeField] private GameObject botVisual;

    private float walkRadius;
    private IState<Bot> currentState;

    protected override void OnInit()
    {
        base.OnInit();
        OnRandomColorAndWeapon();
        walkRadius = 10f;
        ChangeState(new IdleState());
        SetUpWeapon();
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState != null)
            currentState.OnExecute(this);
    }
    public void Move(Vector3 target)
    {
        if (!agent.isOnNavMesh) return;
        Vector3 moveDirection = new Vector3(target.x, target.y, target.z);
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

    protected override void OnDeath()
    {
        base.OnDeath();
        StartCoroutine(OnBotDeath());
    }

    private IEnumerator OnBotDeath()
    {
        yield return new WaitForSeconds(2f);
        OnHideVisual(true);
        ChangeState(new IdleState());
        yield return new WaitForSeconds(2f);
        OnHideVisual(false);
        isDead = false;
        ChangeState(new MoveState());
    }
    public override void OnStartGame()
    {
        base.OnStartGame();
        ChangeState(new MoveState());
    }
    public override void OnEndGame()
    {
        base.OnEndGame();
        Move(TF.position);
        ChangeState(new IdleState());
    }

    public override void OnPrepareGame()
    {
        base.OnPrepareGame();
        ChangeState(new IdleState());
    }
    public override void OnSetting()
    {
        Move(TF.position);
        ChangeState(new IdleState());
    }

    protected void OnHideVisual(bool isHide) => botVisual.SetActive(!isHide);

    public Vector3 RandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        else 
            RandomPosition();
        
        return finalPosition;
    }

    private void OnRevive()
    {
        OnRandomColorAndWeapon();
        Score = 0;
    }

    private void OnRandomColorAndWeapon()
    {
        int randomColor = Random.Range(2, 13);
        colorRenderer.material = GameManager.Ins.GetColorMaterial((ColorEnum)randomColor);
        int randomWeapon = Random.Range(1, 10);
        currentWeapon = GameManager.Ins.GetWeapon((WeaponType)randomWeapon);
    }
}
