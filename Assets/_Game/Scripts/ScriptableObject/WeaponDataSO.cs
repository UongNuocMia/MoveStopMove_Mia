using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/WeaponsList")]

public class WeaponDataSO : ScriptableObject
{
    public List<Weapon> weaponList;

    public Weapon GetWeapon(int id) => weaponList[(id)];

}
