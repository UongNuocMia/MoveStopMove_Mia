using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/CharacterStats")]

public class CharacterConfigSO : ScriptableObject
{
    public float Health;
    public float Speed;
    public float AttackRange;
    public float AttackSpeed;
}
