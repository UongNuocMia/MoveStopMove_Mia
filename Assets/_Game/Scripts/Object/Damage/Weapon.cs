using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private GameObject visualWeapon;
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
        if (bullet == null)
        {
            bullet = (Bullet)SimplePool.Spawn(bulletPrefabs, owner.ShootPoint.position, Quaternion.identity);
            bullet.SetOwner(owner);
        }
        bullet.SetPosition(owner.ShootPoint.position);
        bullet.OnFire();
    }
    protected void OnHideVisual(bool isHide) => visualWeapon.SetActive(isHide);

}
