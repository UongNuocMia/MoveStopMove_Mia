using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EffectList")]

public class EffectDataSO : ScriptableObject
{
    public List<Effect> effectList;

    public Effect GetEffect(int id) => effectList[(id)];
}
