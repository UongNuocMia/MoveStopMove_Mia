
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
    [SerializeField] protected AttackArea attackArea;
    private string currentAnimName;
    protected bool isMoving;
    protected bool isEndGame;
    protected bool isAttacked;
    protected bool isHadTarget;
    protected float speed;
    protected float health;
    protected EBoosterType currentBooster;
    protected Weapon currentWeapon;
    protected Weapon currentWeaponPrefab;
    protected GameObject currentHead;
    protected GameObject currentHeadPrefab;
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai

    public bool IsDead { protected set; get; }
    public int Score { protected set; get; } = 0;
    public float attackSpeed { protected set; get; }
    public float attackRange { protected set; get; }
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
        if (currentWeapon != null)
            return;
        Weapon weapon = Instantiate(currentWeaponPrefab,weaponPoint);
        currentWeapon = weapon;
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
        if (Target != null && !Target.IsDead && CharacterInAreaList.Contains(Target))
            return Target;
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
        health -= 1;
        if (health <= 0)
            OnDeath();
        else
            AudioManager.Ins.PlaySFX(ESound.TargetHitted);

    }
    protected virtual void OnDeath()
    {
        ChangeAnim(Constants.ISDEAD_ANIM);
        IsDead = true;
        AudioManager.Ins.PlaySFX(ESound.TargetDie);
    }
    private void Attack()
    {
        AudioManager.Ins.PlaySFX(ESound.ThrowWeapon);
        ChangeAnim(Constants.ISATTACK_ANIM);
        currentWeapon.Fire();
        isAttacked = true;
        float time = Utilities.GetTimeCurrentAnim(anim, Constants.ATTACK_ANIM);
        Invoke(nameof(ChangeToIdle), time);
    }
    private void ChangeToIdle()
    {
        if (IsDead) return;
        ChangeAnim(Constants.ISIDLE_ANIM);
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
        ChangeAnim(Constants.ISIDLE_ANIM);
    }
    public virtual void OnEndGame()
    {
        isEndGame = true;
        agent.enabled = false;
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
    public virtual void OnKillSuccess(Character character)
    {
        Score += 2;
        CharacterInAreaList.Remove(character);
        LevelManager.Ins.SetCharacterRemain();
        UIManager.Ins.ShowNoti();
    }
}
