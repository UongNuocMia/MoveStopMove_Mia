using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Data/Level Config")]
public class LevelConfig : ScriptableObject
{
    public int ID;
    public ELevelType Type;
    public float Time;
}