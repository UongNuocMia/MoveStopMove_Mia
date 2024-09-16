using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Character character;
    private void Start()
    {
        character = transform.parent.GetComponent<Character>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            Character enemy = other.GetComponent<Character>();
            character.OnEnemyGetInArea(enemy);
        }
        if (character is Player && other.CompareTag(Constants.OBSTACLE_TAG))
        {
            Debug.Log("here");
            Obstacle obstacle = other.GetComponentInParent<Obstacle>();
            obstacle.IsPlayerNear(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            Character enemy = other.GetComponent<Character>();
            character.OnEnemyGetOutArea(enemy);
        }
        if (character is Player && other.CompareTag(Constants.OBSTACLE_TAG))
        {
            Debug.Log("here");
            Obstacle obstacle = other.GetComponentInParent<Obstacle>();
            obstacle.IsPlayerNear(false);
        }
    }
}
