
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;
public class Character : GameUnit
{
    [SerializeField] private SkinnedMeshRenderer colorRenderer;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform hairPoint;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Transform weaponPoint;
    [SerializeField] protected CharacterConfigSO characterConfigSO;
    [SerializeField] protected Weapon weaponPrefab; // for test
    private string currentAnimName;
    protected bool isMoving;
    protected bool isEndGame;
    protected bool isAttack;
    protected float speed;
    protected float health;
    protected BoosterType currentBooster;
    protected Weapon currentWeapon;
    protected List<Character> characterInArea = new();
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai
    public int Score { protected set; get; } = 0;
    public Character target { private set; get; }

    public Transform ShootPoint => shootPoint;
   

    private void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        Score = 0;
        speed = characterConfigSO.speed;
        health = characterConfigSO.health;
        currentBooster = BoosterType.None;
        agent = GetComponent<NavMeshAgent>();
        SetUpWeapon();
    }

    private void SetUpWeapon()
    {
        Weapon weapon = Instantiate(weaponPrefab, weaponPoint);
        weapon.SetOwner(this);
        currentWeapon = weapon;
    }

    public void OnEnemyGetInArea(Character character)
    {
        characterInArea.Add(character);
    }

    public void OnEnemyGetOutArea(Character character)
    {
        if (characterInArea.Contains(character))
        {
            characterInArea.Remove(character);
        }
    }

    public void OnStopMoving()
    {
        if (isAttack)
            return;
        if (characterInArea.Count == 0) return;
        target = FindNearstEnemy();
        if (target == null) return;
        TF.LookAt(target.TF);
        Attack();
    }

    private Character FindNearstEnemy()
    {
        float distance = Vector3.Distance(TF.position, characterInArea[0].TF.position);
        if (characterInArea.Count == 1)
            return characterInArea[0];
        Character character = null;
        foreach (var characterNear in characterInArea)
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

    private void OnDeath()
    {
        Debug.Log("die");

    }

    private void Attack()
    {
        ChangeAnim(Constants.ATTACK_ANIM);
        currentWeapon.Fire();
        isAttack = true;
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
        characterInArea.Remove(character);
        LevelManager.Ins.SetCharacterRemain();
        UIManager.Ins.ShowNoti();
    }
}
