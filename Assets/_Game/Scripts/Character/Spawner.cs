using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Bot botPrefab;
    [SerializeField] private Player playerPrefab;

    private Player player;
    private List<Vector3> randomPositionList = new();
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
            for (int i = 0; i < LevelManager.Ins.CharacterNumb; i++)
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

    public List<Vector3> RandomPosition(Transform startPoint)
    {
        List<Vector3> randomList = new();
        int rows = 12;
        int columns = 9;
        float spacing = 2f;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 newPosition = startPoint.position - new Vector3(-(i * spacing), 0, j * spacing);
                randomList.Add(newPosition);
            }
        }
        Utilities.Shuffle(randomList);
        return randomList;
    }
    private Bot SetUpBot(Vector3 position)
    {
        Bot bot = (Bot)SimplePool.Spawn(this.botPrefab, position, Quaternion.identity);
        return bot;
    }
    private Player SetUpPlayer(Vector3 position)
    {
        Player player = (Player)SimplePool.Spawn(this.playerPrefab, position, Quaternion.identity);
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
