using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/HeadDataSO")]

public class HeadDataSO : ScriptableObject
{
    public List<GameObject> headsList;
    public GameObject GetHead(HeadType headType)
    {
        return headsList[(int)headType];
    }
}
