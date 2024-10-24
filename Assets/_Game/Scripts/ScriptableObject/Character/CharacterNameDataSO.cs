using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/CharacterNameList")]

public class CharacterNameDataSO : ScriptableObject
{
    public List<string> NameList;

    public string GetName(int id) => NameList[(id)];

}
