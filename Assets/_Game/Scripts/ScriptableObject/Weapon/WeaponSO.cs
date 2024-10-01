using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string Name;
    public float AttackRange;
    public float AttackSpeed;
    public Sprite Icon;
    public EWeaponType type;
}
