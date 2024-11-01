using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;




public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<Level> levelList;

    private int CharacterRemain;
    private float reviveTime = 2f;
    private float onGroundTime = 2f;

    private Level currentLevel = null;

    public int TotalLevelNum => levelList.Count;
    public int CharacterNumbOfThisLevel { private set; get; } = 0;
    public int MaxCharacterOnStage { private set; get; }
    public float TimeRemain { private set; get; } = 0;
    public ELevelType CurrentLevelType { private set; get; }
    public List<Vector3> PositionList { private set; get; }
    public List<Bot> BotList { private set; get; }
    public List<Character> CharacterList { private set; get; }

    public void OnLoadMap()
    {
        SetUpMap();
        GenarateCharacters();
        BotList = GetListBot();
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
        CharacterNumbOfThisLevel = currentLevel.MaxCharacter;
        TimeRemain = currentLevel.Time;
        CurrentLevelType = currentLevel.LevelType;
        CharacterRemain = CharacterNumbOfThisLevel;
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
        BotList = Spawner.Ins.BotList;
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

    public void OnReviveBot(Bot bot)
    {
        StartCoroutine(RespawnBot(bot));
    }
    private IEnumerator RespawnBot(Bot bot)
    {
        yield return new WaitForSeconds(onGroundTime);
        bot.OnHideVisual(true);
        reviveTime -= Time.deltaTime;
        yield return new WaitForSeconds(reviveTime);
        if (GetCharacterRemain() > GetCharacterOnGround())
        {
            if (!GameManager.IsState(GameState.GamePlay)) yield break;
            bot.OnRevive();
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
        {
            GameManager.Ins.IsPlayerWin = !GameManager.Ins.Player.IsDead;
            GameManager.Ins.ChangeState(GameState.Finish);
        }

    }

    public Vector3 GetRandomPosition(Vector3 currentPosition)
    {
        Vector3 newPosition = currentLevel.RandomPosition(currentPosition);
        return newPosition;
    }

    public List<Bot> GetListBot()
    {
        return BotList;
    }
}
