using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HatDataSO")]

public class HatDataSO : ScriptableObject
{
    public List<GameObject> hatsList;
    public GameObject GetHat(int id) => hatsList[id];
}
