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
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            Character enemy = other.GetComponent<Character>();
            character.OnEnemyGetOutArea(enemy);
        }
    }
}
