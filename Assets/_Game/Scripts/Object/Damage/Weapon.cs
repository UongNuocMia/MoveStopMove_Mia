using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit, IInteractable
{
    [SerializeField] private GameObject visualBooster;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Bullet bullet;
    private Enum.WeaponEnum weaponEnum;
    private Character owner;



    public void SetOwner(Character character)
    {
        owner = character;
    }

    private void OnFire()
    {
        bullet.SetOwner(owner);
    }

    private void Start()
    {

    }

    protected void OnHideVisual(bool isHide) => visualBooster.SetActive(isHide);
    protected void OnHideCollision(bool isHide) => boxCollider.enabled = !isHide;

    public Transform GetTransform()
    {
        return TF;
    }

    public void Interact(Character character)
    {
        character.TakeDamage();
        OnHideVisual(true);
        OnHideCollision(true);
    }

}
