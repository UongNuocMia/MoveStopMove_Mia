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
    private float maxWalkDistance = 15f;
    private IState<Bot> currentState;

    //Bot Type - Wanderer; Aggresive

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        SetUpWeapon();
        attackArea.SetScale(AttackRange);
    }
    protected override void OnDeath(Character character)
    {
        base.OnDeath(character);
        ChangeState(new DeathState());
        GameManager.Ins.RemoveName(CharacterName);
        nameText.SetText("");
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
        CharacterName = GameManager.Ins.GetRandomName();
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
        GameManager.Ins.RemoveAllName();
        if (!IsDead)
            ChangeState(new IdleState());
    }
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();
        OnHideTargetSprite(true);
        ChangeState(new IdleState());
        OnHideVisual(false);
    }

    private void Update()
    {
        currentState?.OnExecute(this);
    }

    public void Move(Vector3 target)
    {
        if (!agent.isOnNavMesh) return;
        isMoving = true;
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
        int maxAttempts = 10;
        Vector3 finalPosition = Vector3.zero;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = TF.position + Random.insideUnitSphere * maxWalkDistance;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, Random.Range(0f, maxWalkDistance), NavMesh.AllAreas))
            {
                finalPosition = hit.position;
                break;
            }
        }
        return finalPosition;
    }
    public void OnRevive()
    {
        health = characterConfigSO.Health;
        ResetSize();
        SetPosition(LevelManager.Ins.GetRandomPosition(TF.position));
        OnHideVisual(false);
        OnHideCollider(false);
        IsDead = false;
        Score = 0;
        ChangeState(new MoveState());
        waypoint_Indicator.enabled = true;
        waypoint_Indicator.textDescription = Score.ToString();
        CharacterName = GameManager.Ins.GetRandomName();
        SetName();
    }

    public void SetMoving(bool isBotMoving)
    {
        isMoving = isBotMoving;
    }

}
