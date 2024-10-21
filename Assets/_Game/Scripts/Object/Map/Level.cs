using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelDataSO;

    private float mapWidth;
    private float mapHeight;
    public int MaxCharacter { private set; get; }
    public int MaxCharacterOnStage { private set; get; }
    public float Time{ private set; get; }
    public ELevelType LevelType { private set; get; }
    public List<Vector3> RandomPositionList { private set; get; } = new();
    

    public void OnInit()
    {
        LevelType = levelDataSO.LevelType;
        Time = levelDataSO.Time;
        MaxCharacter = levelDataSO.MaxCharacter;
        mapWidth = levelDataSO.MapWidth;
        mapHeight= levelDataSO.MapHeight;
        MaxCharacterOnStage = levelDataSO.MaxCharacter;
        if (MaxCharacterOnStage > 10)
            MaxCharacterOnStage = 10;
        SetRandomPositionList();
    }
    public void SetRandomPositionList()
    {
        while (RandomPositionList.Count < MaxCharacter)
        {
            Vector3 randomPosition = new (Random.Range(-mapWidth, mapWidth), 1, Random.Range(-mapHeight, mapHeight));
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                if (!RandomPositionList.Contains(hit.position) && IsPointFarEnoughFromOthers(hit.position))
                    RandomPositionList.Add(hit.position);
                if (RandomPositionList.Count >= MaxCharacterOnStage)
                    return;
            }
        }
    }
    public Vector3 RandomPosition(Vector3 currentPosition)
    {
        Vector3 newPosition = new (currentPosition.x + Random.Range(-mapWidth, mapWidth),
                                          0, currentPosition.z + Random.Range(-mapHeight, mapHeight));
        if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            RandomPositionList.Add(hit.position);
        }
        return newPosition;
    }
    bool IsPointFarEnoughFromOthers(Vector3 point)
    {
        float minDistance = 4f;
        foreach (Vector3 existingPoint in RandomPositionList)
        {
            if (Vector3.Distance(point, existingPoint) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}
