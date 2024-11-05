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
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            Character enemy = Cache.GetCharacter(other);
            character.OnEnemyGetInArea(enemy);
            if (character is Player && GameManager.IsState(GameState.GamePlay) && !enemy.IsDead)
                Cache.GetBot(other).OnHideTargetSprite(false);

        }
        if (character is Player && other.CompareTag(Constants.OBSTACLE_TAG))
        {
            Obstacle obstacle = other.GetComponentInParent<Obstacle>();
            obstacle.IsPlayerNear(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            Character enemy = Cache.GetCharacter(other);
            character.OnEnemyGetOutArea(enemy);
            if (character is Player)
                Cache.GetBot(other).OnHideTargetSprite(true);
        }
        if (character is Player && other.CompareTag(Constants.OBSTACLE_TAG))
        {
            Obstacle obstacle = other.GetComponentInParent<Obstacle>();
            obstacle.IsPlayerNear(false);
        }
    }
}
