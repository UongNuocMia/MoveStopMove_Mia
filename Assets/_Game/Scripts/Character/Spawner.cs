using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Bot botPrefab;
    [SerializeField] private Player playerPrefab;

    private Player player;
    public List<Bot> BotList { private set; get; } = new();
    public List<Character> CharacterList { private set; get; } = new();

    public void GenarateCharacter(List<Vector3> positionList, int indexOfPlayer)
    {
        if(CharacterList.Count > 0)
        {
            for (int i = 0; i < CharacterList.Count; i++)
            {
                CharacterList[i].SetPositionAndRotation(positionList[i], Quaternion.Euler(Vector3.zero));
            }
        }
        else
        {
            for (int i = 0; i < positionList.Count; i++)
            {
                Character character;
                if (indexOfPlayer == i)
                {
                    character = SetUpPlayer(positionList[i]);
                    player = (Player)character;
                }
                else
                {
                    character = SetUpBot(positionList[i]);
                    BotList.Add((Bot)character);
                }
                CharacterList.Add(character);
            }
        }
    }
    private Bot SetUpBot(Vector3 position)
    {
        Bot bot = SimplePool.Spawn<Bot>(this.botPrefab, position, Quaternion.identity);
        return bot;
    }
    private Player SetUpPlayer(Vector3 position)
    {
        Player player = SimplePool.Spawn<Player>(this.playerPrefab, position, Quaternion.identity);
        return player;
    }
    public Player GetPlayer()
    {
        return player;
    }
    public List<Bot> GetListBot()
    {
        return BotList;
    }
}
