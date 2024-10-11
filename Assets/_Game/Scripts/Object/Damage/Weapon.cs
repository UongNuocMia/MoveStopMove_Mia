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
    public float AttackRange { private set; get; }
    public float AttackSpeed { private set; get; }

    public void SetOwner(Character character)
    {
        owner = character;
    }
    public void OnInit()
    {
        AttackRange = weaponSO.AttackRange;
        AttackSpeed = weaponSO.AttackSpeed;
    }
    public void Fire()
    {
        //OnHideVisual(true);
        if (bullet == null)
        {
            bullet = SimplePool.Spawn<Bullet>(bulletPrefabs, owner.ShootPoint.position, Quaternion.identity);
            bullet.SetOwner(owner);
            bullet.SetAttackSpeed(owner.AttackSpeed);
        }       
        bullet.SetPosition(owner.ShootPoint.position);
        bullet.OnFire();
    }
    //public void OnHideVisual(bool isHide) => weaponVisual.SetActive(!isHide);

}
