using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{ 
    [SerializeField] private GameObject botVisual;
    [SerializeField] private GameObject targetSprite;
    [SerializeField] private CapsuleCollider botCollider;
    private float walkRadius;
    private IState<Bot> currentState;

    //Bot Type - Wanderer; Aggresive

    protected override void OnInit()
    {
        base.OnInit();
        OnRandomItem();
        walkRadius = 5f;
        ChangeState(new IdleState());
        SetUpWeapon();
        attackArea.SetScale(AttackRange);
    }

    // Update is called once per frame
    private void Update()
    {
        currentState?.OnExecute(this);
    }
    public void Move(Vector3 target)
    {
        if (!agent.isOnNavMesh) return;
        Vector3 moveDirection = new(target.x, target.y, target.z);
        Vector3 destination = moveDirection; 
        agent.speed = speed;
        agent.SetDestination(destination);
        TF.forward = Vector3.Slerp(TF.forward, moveDirection, 0);
        ChangeAnim(Constants.ISRUN_ANIM);
    }
    public void ChangeState(IState<Bot> newState)
    {
        currentState?.OnExit(this);

        currentState = newState;

        currentState?.OnEnter(this);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(new DeathState());
    }
    public override void OnStartGame()
    {
        base.OnStartGame();
        OnHideCollider(false);
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
        OnHideVisual(false);
    }
    
    public void OnHideVisual(bool isHide) => botVisual.SetActive(!isHide);
    public void OnHideCollider(bool isHide) => botCollider.enabled = !isHide;
    public void OnHideTargetSprite(bool isHide) => targetSprite.SetActive(!isHide);

    public Vector3 RandomPosition()
    {
        Vector3 randomDirection = TF.position + Random.insideUnitSphere * walkRadius;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkRadius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void OnRevive()
    {
        SetPosition(LevelManager.Ins.GetRandomPosition(TF.position));
        OnHideVisual(false);
        OnHideCollider(false);
        IsDead = false;
        Score = 0;
        ChangeState(new MoveState());
    }
    protected override void SetUpAccessories()
    {
        base.SetUpAccessories();
        pantRenderer.material = GameManager.Ins.GetRandomPant();
        if (currentHat != null)
        {
            Destroy(currentHat.gameObject);
        }
        currentHatPrefab = GameManager.Ins.GetRandomHat();
        Hat hat = Instantiate(currentHatPrefab, hatPoint);
        currentHat = hat;
    }
    private void OnRandomItem()
    {
        colorRenderer.material = GameManager.Ins.GetRandomColor();
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        Weapon weapon = GameManager.Ins.GetRandomWeapon();
        currentWeaponPrefab = weapon;
        SetUpAccessories();
    }
}
