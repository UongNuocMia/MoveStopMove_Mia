
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
public class Character : GameUnit
{
    [SerializeField] private SkinnedMeshRenderer colorRenderer;

    [SerializeField] protected Animator anim;
    [SerializeField] protected Weapon currentWeapon;
    [SerializeField] protected CharacterConfigSO characterConfigSO;

    private string currentAnimName;
    protected bool isMoving;
    protected bool isEndGame;
    protected bool isAttack;
    protected float speed;
    protected float health;
    protected float interactRange;
    protected Enum.BoosterEnum currentBooster;
    protected List<Character> characterInArea = new();
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai
    public int Score { protected set; get; } = 0;
    public Character target { private set; get; }
    public ColorEnum CharacterColorEnum { protected set; get; }
   

    private void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        Score = 0;
        interactRange = 3f;
        speed = characterConfigSO.speed;
        health = characterConfigSO.health;
        currentBooster = Enum.BoosterEnum.None;
        agent = GetComponent<NavMeshAgent>();
    }

    private void SetUpWeapon()
    {
        currentWeapon.SetOwner(this);
    }

    [SerializeField] private float rotationSpeed = 0.2f;
    public void OnEnemyGetInArea(Character character)
    {
         // just for test
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

    public void Scan()
    {
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
        //tèo luôn
        Debug.Log("die");
    }

    private void Attack()
    {
        isAttack = true;
        ChangeAnim(Constants.ATTACK_ANIM);
    }

    private void OnTriggerEnter(Collider collider)
    {    
        if (collider.CompareTag(Constants.INTERACTABLE_TAG))
        {
            IInteractable interactableObject = collider.GetComponent<IInteractable>();
            interactableObject.Interact(this);
        }
    }

    public void GetBooster(Enum.BoosterEnum booster)
    {
        currentBooster = booster;
        OnGetBooster(currentBooster);
        currentBooster = Enum.BoosterEnum.None;
    }

    #region Status
    public void OnChangeColor(Material material, ColorEnum colorEnum)
    {
        colorRenderer.material = material;
        CharacterColorEnum = colorEnum;
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
    public void OnResult(Transform transform, int rank)
    {
        TF.position = transform.position;
        TF.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        if (rank == 0)
            ChangeAnim(Constants.WIN_ANIM);
        //else
        //    ChangeAnim(Constants.LOSE_ANIM);
    }

    private void OnGetBooster(Enum.BoosterEnum boosterEnum)
    {
        switch (boosterEnum)
        {
            case Enum.BoosterEnum.None:
                break;
            case Enum.BoosterEnum.KingSpeed:
                speed++;
                break;
            case Enum.BoosterEnum.Hulk:
                SetScale(0.1f);
                break;
            case Enum.BoosterEnum.Fly:
                break;
            case Enum.BoosterEnum.WeaponScale:
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
        LevelManager.SetCharacterRemain();
        UIManager.ShowNoti();
    }
}
