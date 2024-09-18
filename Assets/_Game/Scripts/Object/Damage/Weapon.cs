using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private GameObject weaponVisual;
    [SerializeField] private Bullet bulletPrefabs;
    private Bullet bullet;
    private Character owner;
    private WeaponType weaponEnum;

    public void SetOwner(Character character)
    {
        owner = character;
    }
    public void Fire()
    {
        OnHideVisual(true);
        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefabs);
            bullet.SetOwner(owner);
        }
        
        bullet.transform.position = owner.ShootPoint.position;
        bullet.OnFire();
    }

    public void OnOwnerDeath()
    {

    }
    public void OnHideVisual(bool isHide) => weaponVisual.SetActive(!isHide);

}
