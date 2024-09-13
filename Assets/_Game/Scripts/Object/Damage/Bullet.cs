using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Character owner;

    public void SetOwner(Character character)
    {
        owner = character;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.CHARACTER_TAG))
        {
            Character character = other.GetComponent<Character>();
            owner.OnKillSucess(character);
        }
    }
}
