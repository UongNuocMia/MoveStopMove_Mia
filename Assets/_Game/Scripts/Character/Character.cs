
using System;
using System.Collections;
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
    [SerializeField] protected Transform characterVisual;
    [SerializeField] protected AttackArea attackArea;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected CharacterConfigSO characterConfigSO;
    [SerializeField] protected Waypoint_Indicator waypoint_Indicator;

    private float timer = 0.13f;
    private float upSize = 0;
    private string currentAnimName;
    protected bool isMoving;
    protected bool isEndGame;
    protected bool isAttacked;
    protected bool isHadTarget;
    protected float speed;
    protected float health;
    protected EBoosterType currentBooster;
    protected Hat currentHat;
    protected Hat currentHatPrefab;
    protected Weapon currentWeapon;
    protected Weapon currentWeaponPrefab;
    protected NavMeshAgent agent; // player cung dung de tranh truong hop bay ra ngoai

    public event Action OnScaleUp;

    public bool IsDead { protected set; get; }
    public int Score { protected set; get; } = 0;
    public float AttackSpeed { protected set; get; }
    public float AttackRange { protected set; get; }
    public string CharacterName{ protected set; get; }

    public Character Target { private set; get; }
    public Character Killer { private set; get; }
    public List<Character> CharacterInAreaList { private set; get; } = new();
    public Transform ShootPoint => shootPoint;
    public Transform CharacterVisual => characterVisual;


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
        WaypointSetting();
        nameText.SetText("");
    }
    protected virtual void SetUpAccessories()
    {

    }
    protected virtual void OnRandomAppearance()
    {
        colorRenderer.material = GameManager.Ins.GetRandomColor();
    }
    protected virtual void OnDeath(Character killer)
    {
        IsDead = true;
        Killer = killer;
        ChangeAnim(Constants.ISDEAD_ANIM);
        AudioManager.Ins.PlaySFX(ESound.TargetDie);
        waypoint_Indicator.enabled = false;
    }

    protected virtual void SetName()
    {
        nameText.SetText(CharacterName);
        nameText.color = GetCharacterColor();
    }

    public virtual void OnStartGame()
    {
        agent.enabled = true;
        isEndGame = false;
        waypoint_Indicator.enabled = true;
        SetName();
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
        nameText.SetText("");
        CharacterInAreaList.Clear();
        waypoint_Indicator.enabled = false;
    }
    public virtual void OnKillSuccess(Character character)
    {
        Score += 2;
        waypoint_Indicator.textDescription = Score.ToString();
        CharacterInAreaList.Remove(character);
        LevelManager.Ins.SetCharacterRemain();
        UIManager.Ins.ShowNoti();
        if(UnityEngine.Random.Range(0, 10) < 4)
        {
            OnScaleUp?.Invoke();
            CharacterSizeUp();
        }
    }

    private void Attack()
    {
        timer -= Time.deltaTime;

        ChangeAnim(Constants.ISATTACK_ANIM);
        if (timer <= 0)
        {
            currentWeapon.OnHideVisual(true);
            isAttacked = true;
            AudioManager.Ins.PlaySFX(ESound.ThrowWeapon);
            currentWeapon.Fire();
            float resetAnimTime = 0.8f;
            Debug.Log("time___" + resetAnimTime);
            StartCoroutine(ChangeToIdle(resetAnimTime));
            timer = 0.13f;
        }
    }
    private IEnumerator ChangeToIdle(float time)
    {
        yield return new WaitForSeconds(time);
        currentWeapon.OnHideVisual(false);

        if (!IsDead || GameManager.IsState(GameState.GamePlay) && !isMoving)
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

    private void CharacterSizeUp()
    {
        upSize += 0.1f;

        characterVisual.localScale = new Vector3(characterVisual.localScale.x + upSize, 
            characterVisual.localScale.y + upSize, characterVisual.localScale.y + upSize);

        attackArea.transform.localScale = new Vector3(attackArea.transform.localScale.x + upSize,
            attackArea.transform.localScale.y + upSize, attackArea.transform.localScale.z + upSize);

        shootPoint.position = new Vector3(shootPoint.position.x, shootPoint.position.y, upSize);
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

    protected void WaypointSetting()
    {
        waypoint_Indicator.textColor = GetCharacterColor();
        waypoint_Indicator.onScreenSpriteColor = GetCharacterColor();
        waypoint_Indicator.offScreenSpriteColor = GetCharacterColor();
        waypoint_Indicator.textDescription = Score.ToString();
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
        if (CharacterInAreaList.Count == 0 || isAttacked || IsDead) return false;
        Target = FindNearstEnemy();
        if (Target == null) return false;
        return true;
    }
    public void TakeDamage(Character killer)
    {
        health -= 1;
        if (health <= 0)
            OnDeath(killer);
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

    public Color GetCharacterColor()
    {
        return colorRenderer.material.color;
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
