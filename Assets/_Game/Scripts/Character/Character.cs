
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Character : GameUnit
{
    [SerializeField] protected SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform hairPoint;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Transform weaponPoint;
    [SerializeField] protected CharacterConfigSO characterConfigSO;
    private string currentAnimName;
    protected bool isDead;
    protected bool isMoving;
    protected bool isAttack;
    protected bool isEndGame;
    protected float speed;
    protected float health;
    protected BoosterType currentBooster;
    protected Weapon currentWeapon;
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai

    public int Score { protected set; get; } = 0;
    public Character target { private set; get; }
    public List<Character> characterInAreaList { private set; get; } = new();
    public Transform ShootPoint => shootPoint;
   

    private void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        Score = 0;
        isDead = false;
        speed = characterConfigSO.speed;
        health = characterConfigSO.health;
        currentBooster = BoosterType.None;
        agent = GetComponent<NavMeshAgent>();
    }

    protected void SetUpWeapon()
    {
        Instantiate(currentWeapon, weaponPoint);
        currentWeapon.OnHideVisual(false);
        currentWeapon.SetOwner(this);
    }

    public void OnEnemyGetInArea(Character character)
    {
        if (character.isDead)
            return;
        characterInAreaList.Add(character);
    }

    public void OnEnemyGetOutArea(Character character)
    {
        if (characterInAreaList.Contains(character))
        {
            characterInAreaList.Remove(character);
        }
    }

    public void OnStopMoving()
    {
        ChangeAnim(Constants.IDLE_ANIM);
        OnPrepareAttack();
    }

    public void OnPrepareAttack()
    {
        if (characterInAreaList.Count == 0 || isAttack) return;
        target = FindNearstEnemy();
        if (target == null) return;
        TF.LookAt(target.TF);
        Attack();
    }

    private Character FindNearstEnemy()
    {
        float distance = Vector3.Distance(TF.position, characterInAreaList[0].TF.position);
        if (characterInAreaList.Count == 1)
            return characterInAreaList[0];
        Character character = null;
        foreach (var characterNear in characterInAreaList)
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
        Debug.Log("die");
        ChangeAnim(Constants.DEAD_ANIM);
        currentWeapon.OnOwnerDeath();
        isDead = true;
    }

    private void Attack()
    {
        ChangeAnim(Constants.ATTACK_ANIM);
        currentWeapon.Fire();
        isAttack = true;
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

    public void GetBooster(BoosterType booster)
    {
        currentBooster = booster;
        OnGetBooster(currentBooster);
        currentBooster = BoosterType.None;
    }

    #region Status
    public void OnChangeColor(Material material, ColorEnum colorEnum)
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


    private void OnGetBooster(BoosterType boosterEnum)
    {
        switch (boosterEnum)
        {
            case BoosterType.None:
                break;
            case BoosterType.KingSpeed:
                speed++;
                break;
            case BoosterType.Hulk:
                SetScale(0.1f);
                break;
            case BoosterType.Fly:
                break;
            case BoosterType.WeaponScale:
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
        characterInAreaList.Remove(character);
        LevelManager.Ins.SetCharacterRemain();
        UIManager.Ins.ShowNoti();
    }
}
