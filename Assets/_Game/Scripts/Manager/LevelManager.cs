using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;




public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Level> levelList;

    private int CharacterRemain;
    private Level currentLevel = null;

    public int TotalLevelNum => levelList.Count;
    public int CharacterNumb { private set; get; } = 0;
    public int MaxCharacterOnStage { private set; get; }
    public float TimeRemain { private set; get; } = 0;
    public ELevelType CurrentLevelType { private set; get; }
    public List<Vector3> PositionList { private set; get; }
    public List<Character> CharacterList { private set; get; }

    public void OnLoadMap()
    {
        SetUpMap();
        GenarateCharacters();
    }

    private void SetUpMap()
    {
        DestroyMap();
        int levelID = GameManager.Ins.Level;
        currentLevel = Instantiate(levelList[levelID]);
        currentLevel.OnInit();
        currentLevel.transform.position = Vector3.zero;
        MaxCharacterOnStage = currentLevel.MaxCharacterOnStage;
        PositionList = currentLevel.RandomPositionList;
        CharacterNumb = currentLevel.MaxCharacter;
        TimeRemain = currentLevel.time;
        CurrentLevelType = currentLevel.levelType;
        CharacterRemain = CharacterNumb;
    }

    private void DestroyMap()
    {
        if (currentLevel == null)
            return;
        Destroy(currentLevel.gameObject);
    }

    private void GenarateCharacters()
    {
        int indexOfPlayer = Random.Range(0, MaxCharacterOnStage);
        Spawner.Ins.GenarateCharacter(PositionList, indexOfPlayer);
        CharacterList = Spawner.Ins.CharacterList;
    }

    public void CharactersOnStartGame()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnStartGame();
        }
    }

    public void CharactersOnEndGame()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnEndGame();
        }
    }

    public void CharacterOnPrepare()
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            CharacterList[i].OnPrepareGame();
        }
    }

    public int GetCharacterOnGround()
    {
        int numb = 0;
        for (int i = 0; i < CharacterList.Count; i++)
        {
            if (!CharacterList[i].IsDead)
            {
                numb++;
            }
        }
        return numb;
    }

    public int GetCharacterRemain() => CharacterRemain;

    public void SetCharacterRemain()
    {
        //total character onstage = ??
        CharacterRemain -= 1;
        if (CharacterRemain == 1)
            GameManager.Ins.ChangeState(GameState.Finish);

    }

    public Vector3 GetRandomPosition(Vector3 currentPosition)
    {
        Vector3 newPosition = currentLevel.RandomPosition(currentPosition);
        return newPosition;
    }
}
