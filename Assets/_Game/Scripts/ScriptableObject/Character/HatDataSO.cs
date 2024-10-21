using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HatDataSO")]

public class HatDataSO : ScriptableObject
{
    public List<Hat> hatsList;
    public Hat GetHat(int id) => hatsList[id];
}
