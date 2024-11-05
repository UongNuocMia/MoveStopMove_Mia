using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache
{
    private static Dictionary<Collider, Character> charDict = new();
    private static Dictionary<Collider, Bot> botDict = new();

    public static Character GetCharacter(Collider collider)
    {
        if (!charDict.ContainsKey(collider))
        {
            Character character = collider.GetComponent<Character>();

            charDict.Add(collider, character);
        }
        return charDict[collider];
    }

    public static Bot GetBot(Collider collider)
    {
        if (!botDict.ContainsKey(collider))
            botDict.Add(collider, charDict[collider].GetComponent<Bot>());
        return botDict[collider];
    }
}
