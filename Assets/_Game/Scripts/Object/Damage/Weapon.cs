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
    private WeaponType weaponEnum;
    private float attackRange;
    private float attackSpeed;

    public void SetOwner(Character character)
    {
        owner = character;
    }
    public void OnInit()
    {
        attackRange = weaponSO.attackRange;
        attackSpeed = weaponSO.attackSpeed;
    }
    public void Fire()
    {
        //OnHideVisual(true);
        if (bullet == null)
        {
            bullet = Instantiate(bulletPrefabs);
            bullet.SetOwner(owner);
            bullet.SetStats(weaponSO.attackSpeed, weaponSO.attackRange);
        }       
        bullet.SetPosition(owner.ShootPoint.position);
        bullet.OnFire();
    }
    //public void OnHideVisual(bool isHide) => weaponVisual.SetActive(!isHide);

}
