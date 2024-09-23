using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] private GameObject weaponVisual;
    [SerializeField] private Bullet bulletPrefabs;
    [SerializeField] private WeaponSO weaponSO;
    private Bullet bullet;
    private Character owner;
    private EWeaponType weaponEnum;
    public float AttackRange { private set; get; }
    public float AttackSpeed { private set; get; }

    public void SetOwner(Character character)
    {
        owner = character;
    }
    public void OnInit()
    {
        AttackRange = weaponSO.attackRange;
        AttackSpeed = weaponSO.attackSpeed;
    }

    public void GetStats()
    {

    }
    public void Fire()
    {
        //OnHideVisual(true);
        if (bullet == null)
        {
            bullet = (Bullet)SimplePool.Spawn(bulletPrefabs, owner.ShootPoint.position, Quaternion.identity);
            bullet.SetOwner(owner);
        }       
        bullet.SetPosition(owner.ShootPoint.position);
        bullet.OnFire();
    }
    //public void OnHideVisual(bool isHide) => weaponVisual.SetActive(!isHide);

}
