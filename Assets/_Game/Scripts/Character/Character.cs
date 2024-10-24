
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
public class Character : GameUnit
{
    [SerializeField] protected SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected SkinnedMeshRenderer pantRenderer;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform hatPoint;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Transform weaponPoint;
    [SerializeField] protected GameObject nameGO;
    [SerializeField] protected AttackArea attackArea;
    [SerializeField] protected CharacterConfigSO characterConfigSO;
    [SerializeField] protected TextMeshProUGUI nameText;

    private float timer = 0.3f;
    private string currentAnimName;
    protected bool isMoving;
    protected bool isEndGame;
    protected bool isAttacked;
    protected bool isHadTarget;
    protected float speed;
    protected float health;
    protected string characterName;
    protected EBoosterType currentBooster;
    protected Hat currentHat;
    protected Hat currentHatPrefab;
    protected Weapon currentWeapon;
    protected Weapon currentWeaponPrefab;
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai

    public bool IsDead { protected set; get; }
    public int Score { protected set; get; } = 0;
    public float AttackSpeed { protected set; get; }
    public float AttackRange { protected set; get; }
    public Character Target { private set; get; }
    public List<Character> CharacterInAreaList { private set; get; } = new();
    public Transform ShootPoint => shootPoint;
   
    protected virtual void OnInit()
    {
        Score = 0;
        IsDead = false;
        speed = characterConfigSO.Speed;
        health = characterConfigSO.Health;
        AttackRange = characterConfigSO.AttackRange;
        AttackSpeed = characterConfigSO.AttackSpeed;
        currentBooster = EBoosterType.None;
        agent = GetComponent<NavMeshAgent>();
        OnRandomAppearance();
    }
    protected virtual void SetUpAccessories()
    {

    }
    protected virtual void OnRandomAppearance()
    {
        colorRenderer.material = GameManager.Ins.GetRandomColor();
    }
    protected virtual void OnDeath()
    {
        ChangeAnim(Constants.ISDEAD_ANIM);
        IsDead = true;
        AudioManager.Ins.PlaySFX(ESound.TargetDie);
    }

    protected virtual void SetName()
    {
        nameText.SetText(characterName);
        nameText.color = colorRenderer.material.color;
    }

    public virtual void OnStartGame()
    {
        agent.enabled = true;
        isEndGame = false;
        nameGO.SetActive(true);
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
        nameGO.SetActive(false);
    }
    public virtual void OnKillSuccess(Character character)
    {
        Score += 2;
        CharacterInAreaList.Remove(character);
        LevelManager.Ins.SetCharacterRemain();
        UIManager.Ins.ShowNoti();
    }

    private void Attack()
    {
        timer -= Time.deltaTime;

        ChangeAnim(Constants.ISATTACK_ANIM);
        if (timer <= 0)
        {
            isAttacked = true;
            AudioManager.Ins.PlaySFX(ESound.ThrowWeapon);
            currentWeapon.Fire();
            float time = Utilities.GetTimeCurrentAnim(anim, Constants.ATTACK_ANIM);
            Invoke(nameof(ChangeToIdle), time); //change to couroutine
            timer = 0.3f;
        }

    }
    private void ChangeToIdle()
    {
        if (IsDead) return;
        ChangeAnim(Constants.ISIDLE_ANIM);
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

    protected void SetUpWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        Weapon weapon = Instantiate(currentWeaponPrefab, weaponPoint);
        currentWeapon = weapon;
        currentWeapon.SetOwner(this);
        AttackRange += currentWeapon.AttackRange;
        AttackSpeed += currentWeapon.AttackSpeed;
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
    public void OnPrepareAttack()
    {
        TF.LookAt(Target.TF);
        Attack();
    }
    public bool IsCanAttack()
    {
        if (CharacterInAreaList.Count == 0 || isAttacked) return false;
        Target = FindNearstEnemy();
        if (Target == null) return false;
        return true;
    }
    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
            OnDeath();
        else
            AudioManager.Ins.PlaySFX(ESound.TargetHitted);

    }

    public void GetBooster(EBoosterType booster)
    {
        currentBooster = booster;
        OnGetBooster(currentBooster);
        currentBooster = EBoosterType.None;
    }
    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Constants.INTERACTABLE_TAG))
        {
            IInteractable interactableObject = collider.GetComponent<IInteractable>();
            interactableObject.Interact(this);
        }
    }

}
