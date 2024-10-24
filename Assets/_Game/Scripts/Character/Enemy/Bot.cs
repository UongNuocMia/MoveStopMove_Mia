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
    [SerializeField] private Waypoint_Indicator waypoint_Indicator;
    private float walkRadius;
    private IState<Bot> currentState;

    private string botName;

    //Bot Type - Wanderer; Aggresive

    protected override void OnInit()
    {
        base.OnInit();
        walkRadius = 5f;
        ChangeState(new IdleState());
        SetUpWeapon();
        attackArea.SetScale(AttackRange);
        SetName();
        WaypointSetting();
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        ChangeState(new DeathState());
        GameManager.Ins.RemoveName(characterName);
        waypoint_Indicator.enabled = false;
        nameGO.SetActive(false);
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
    protected override void OnRandomAppearance()
    {
        base.OnRandomAppearance();
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        Weapon weapon = GameManager.Ins.GetRandomWeapon();
        currentWeaponPrefab = weapon;
        SetUpAccessories();
        characterName = GameManager.Ins.GetRandomName();
    }
    public override void OnStartGame()
    {
        base.OnStartGame();
        waypoint_Indicator.enabled = true;
        OnHideCollider(false);
        ChangeState(new MoveState());

    }
    public override void OnEndGame()
    {
        base.OnEndGame();
        waypoint_Indicator.enabled = false;
        Move(TF.position);
        GameManager.Ins.RemoveAllName();
        ChangeState(new IdleState());
    }
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();
        ChangeState(new IdleState());
        OnHideVisual(false);
    }
    public override void OnKillSuccess(Character character)
    {
        base.OnKillSuccess(character);
        waypoint_Indicator.textDescription = Score.ToString();
    }

    private void Update()
    {
        currentState?.OnExecute(this);
    }

    private void WaypointSetting()
    {
        waypoint_Indicator.textColor = colorRenderer.material.color;
        waypoint_Indicator.onScreenSpriteColor = colorRenderer.material.color;
        waypoint_Indicator.offScreenSpriteColor = colorRenderer.material.color;
        waypoint_Indicator.textDescription = Score.ToString();
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
        waypoint_Indicator.enabled = true;
        nameGO.SetActive(true);
        characterName = GameManager.Ins.GetRandomName();
        SetName();
    }

}
