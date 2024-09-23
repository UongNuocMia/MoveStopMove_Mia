
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Character : GameUnit
{
    [SerializeField] protected SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected SkinnedMeshRenderer pantRenderer;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform headPoint;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Transform weaponPoint;
    [SerializeField] protected CharacterConfigSO characterConfigSO;
    private string currentAnimName;
    protected bool isMoving;
    protected bool isAttacked;
    protected bool isEndGame;
    protected float speed;
    protected float health;
    protected float attackRange;
    protected float attackSpeed;
    protected EBoosterType currentBooster;
    protected Weapon currentWeapon;
    protected GameObject currentHeadGO;
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai

    public bool IsDead { protected set; get; }
    public int Score { protected set; get; } = 0;
    public Character Target { private set; get; }
    public List<Character> CharacterInAreaList { private set; get; } = new();
    public Transform ShootPoint => shootPoint;
   
    protected virtual void OnInit()
    {
        Score = 0;
        IsDead = false;
        speed = characterConfigSO.Speed;
        health = characterConfigSO.Health;
        attackRange = characterConfigSO.AttackRange;
        attackSpeed = characterConfigSO.AttackSpeed;
        currentBooster = EBoosterType.None;
        agent = GetComponent<NavMeshAgent>();
    }

    protected void SetUpWeapon()
    {
        Weapon weapon = Instantiate(currentWeapon, weaponPoint);
        currentWeapon = weapon;
        //currentWeapon.OnHideVisual(false);
        currentWeapon.SetOwner(this);
        attackRange += currentWeapon.AttackRange;
        attackSpeed += currentWeapon.AttackSpeed;
    }

    public void OnEnemyGetInArea(Character character)
    {
        if (character.IsDead)
            return;
        isAttacked = false;
        CharacterInAreaList.Add(character);
    }

    public void OnEnemyGetOutArea(Character character)
    {
        if (CharacterInAreaList.Contains(character))
        {
            CharacterInAreaList.Remove(character);
        }
    }

    protected virtual void SetUpAccessories()
    {

    }

    public void OnPrepareAttack()
    {
        TF.LookAt(Target.TF);
        Attack();
    }

    public bool isCanAttack()
    {
        if (CharacterInAreaList.Count == 0 || isAttacked) return false;
        Target = FindNearstEnemy();
        if (Target == null) return false;
        return true;
    }

    private Character FindNearstEnemy()
    {
        float distance = Vector3.Distance(TF.position, CharacterInAreaList[0].TF.position);
        if (CharacterInAreaList.Count == 1)
            return CharacterInAreaList[0];
        Character character = null;
        foreach (var characterNear in CharacterInAreaList)
        {
            if (Vector3.Distance(TF.position, characterNear.TF.position) < distance)
                character = characterNear;
        }
        return character;
    }

    public void TakeDamage()
    {
        health = 0;
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        ChangeAnim(Constants.DEAD_ANIM);
        IsDead = true;
        if(this is Player)
        {
            GameManager.Ins.IsPlayerWin = false;
            GameManager.Ins.ChangeState(GameState.Finish);
        }
    }

    private void Attack()
    {
        ChangeAnim(Constants.ATTACK_ANIM);
        currentWeapon.Fire();
        isAttacked = true;
        float time = Utilities.GetTimeCurrentAnim(anim, "Attack");
        Invoke(nameof(ChangeToIdle), time);
    }

    private void ChangeToIdle()
    {
        ChangeAnim(Constants.IDLE_ANIM);
    }

    private void OnTriggerEnter(Collider collider)
    {    
        if (collider.CompareTag(Constants.INTERACTABLE_TAG))
        {
            IInteractable interactableObject = collider.GetComponent<IInteractable>();
            interactableObject.Interact(this);
        }
    }

    public void GetBooster(EBoosterType booster)
    {
        currentBooster = booster;
        OnGetBooster(currentBooster);
        currentBooster = EBoosterType.None;
    }

    #region Status
    public void OnChangeColor(Material material, EColor colorEnum)
    {
        colorRenderer.material = material;
    }
    public virtual void OnStartGame()
    {
        agent.enabled = true;
        isEndGame = false;
    }
    public virtual void OnPrepareGame()
    {
        OnInit();
        ChangeAnim(Constants.IDLE_ANIM);
    }
    public virtual void OnEndGame()
    {
        isEndGame = true;
        agent.enabled = false;
    }
    public virtual void OnSetting()
    {

    }


    private void OnGetBooster(EBoosterType boosterEnum)
    {
        switch (boosterEnum)
        {
            case EBoosterType.None:
                break;
            case EBoosterType.KingSpeed:
                speed++;
                break;
            case EBoosterType.Hulk:
                SetScale(0.1f);
                break;
            case EBoosterType.Fly:
                break;
            case EBoosterType.WeaponScale:
                currentWeapon.SetScale(0.1f);
                break;
            default:
                break;
        }

    }
    #endregion


    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnKillSucess(Character character)
    {
        Score += 2;
        CharacterInAreaList.Remove(character);
        LevelManager.Ins.SetCharacterRemain();
        UIManager.Ins.ShowNoti();
    }
}
