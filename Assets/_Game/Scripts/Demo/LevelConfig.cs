using UnityEngine;

[CreateAssetMenu(menuName = "Data/Level Config")]
public class LevelConfig : ScriptableObject
{
    public int ID;
    public ELevelType Type;
    public float Time;
}