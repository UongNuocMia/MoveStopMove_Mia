using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/LevelDataSO")]

public class LevelDataSO : ScriptableObject
{
    public int LevelID;
    public ELevelType LevelType;
    public int MaxCharacter;
    public float Time;
    public float MapWidth;
    public float MapHeight;
}
