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
        OnRandom();
        walkRadius = 10f;
        ChangeState(new IdleState());
        SetUpWeapon();
        attackArea.SetScale(attackRange);
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
    public override void OnSetting()
    {
        Move(TF.position);
        ChangeState(new IdleState());
    }

    public void OnHideVisual(bool isHide) => botVisual.SetActive(!isHide);
    public void OnHideCollider(bool isHide) => botCollider.enabled = !isHide;
    public void OnHideTargetSprite(bool isHide) => targetSprite.SetActive(!isHide);

    public Vector3 RandomPosition()
    {
        Vector3 randomDirection = TF.position + Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        else
            RandomPosition();

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
        int randomHeadnPant = Random.Range(1, 9);
        pantRenderer.material = GameManager.Ins.GetPantMaterials((EPantType)randomHeadnPant);
        if (currentHead != null)
        {
            Debug.Log("here bro");
            Destroy(currentHead);
        }
        GameObject headGO = Instantiate(GameManager.Ins.GetHead((EHeadType)randomHeadnPant), headPoint);
        currentHead = headGO;
    }
    private void OnRandom()
    {
        int randomColor = Random.Range(2, 13);
        colorRenderer.material = GameManager.Ins.GetColorMaterial((EColor)randomColor);
        int randomWeapon = Random.Range(1, 10);
        if (currentWeapon != null)
        {
            Debug.Log("here bro 2");
            Destroy(currentWeapon.gameObject);
        }
        Weapon weapon = GameManager.Ins.GetWeapon((EWeaponType)randomWeapon);
        currentWeaponPrefab = weapon;
        SetUpAccessories();
    }
}
