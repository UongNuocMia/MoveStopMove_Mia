using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private List<Material> materialsList;
    [SerializeField] private MeshRenderer obstacleMeshRenderer;

    private void Start()
    {
        obstacleMeshRenderer.material = materialsList[0];
    }
    public void IsPlayerNear(bool isNear)
    {
        obstacleMeshRenderer.material = isNear ? materialsList[1] : materialsList[0];
    }
}
