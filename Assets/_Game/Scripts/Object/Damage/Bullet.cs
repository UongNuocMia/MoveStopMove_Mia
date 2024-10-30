using UnityEngine;

public class Bullet : GameUnit, IInteractable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject bulletVisual;
    [SerializeField] private BoxCollider boxCollider;

    private Effect hitEffect;
    private Character owner;
    private bool isSpawn;
    private float timeExist;
    private float attackSpeed;

    private void OnInit()
    {
        timeExist = 2f;
        OnHideVisual(false);
        OnHideCollision(false);

    }

    public void SetOwner(Character character)
    {
        owner = character;
    }
    public void SetAttackSpeed(float attackSpeed)
    {
        this.attackSpeed = attackSpeed;
    }
    public void OnFire()
    {
        OnInit();
        isSpawn = true;
        rb.velocity = owner.ShootPoint.transform.forward * attackSpeed;
        SetScale(owner.GetCharacterSize());
    }
    private void OnDespawn()
    {
        isSpawn = false;
        rb.velocity = Vector3.zero;
        OnHideVisual(true);
        OnHideCollision(true);
        SetPosition(owner.TF.position);
    }

    private void Update()
    {
        timeExist -= Time.deltaTime;
        if (isSpawn)
        {
            BulletRolling();
        }
        if (isSpawn && timeExist <= 0 || !GameManager.IsState(GameState.GamePlay))
        {
            OnDespawn();
        }
    }
    private void BulletRolling()
    {
        float radius = 1f;
        float rollSpeed = rb.velocity.magnitude / radius;
        // Calculate rotation angle
        float rotationAngle = rollSpeed * Time.deltaTime * 360f;
        TF.Rotate(Vector3.up, rotationAngle);
    }
    public void Interact(Character character)
    {
        if (owner == character || !GameManager.IsState(GameState.GamePlay))
            return;
        character.TakeDamage(owner);
        hitEffect = Instantiate(GameManager.Ins.GetEffect(EEffectType.Hit_By_Bullet_VFX), character.HitEff_Pos);
        if (character.IsDead)
            owner.OnKillSuccess(character);
        OnDespawn();
    }
    protected void OnHideVisual(bool isHide) => bulletVisual.SetActive(!isHide);
    protected void OnHideCollision(bool isHide) => boxCollider.enabled = !isHide;

}
