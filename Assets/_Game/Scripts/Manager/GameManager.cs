using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Setting, Shop }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    [SerializeField] private Transform effectHolder;
    
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private HatDataSO hatDataSO;
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private EffectDataSO effectDataSO;
    [SerializeField] private MaterialsDataSO colorDataSO;
    [SerializeField] private MaterialsDataSO pantDataSO;
    [SerializeField] private CharacterNameDataSO characterNameDataSO;


    private List<string> nameList = new();
    private static GameState gameState = GameState.MainMenu;

    public bool IsNewGame = true;
    public bool IsPlayerWin = true;
    public int Level { private set; get; }
    public int CoinReceive { private set; get; }

    public bool IsMaxLevel { private set; get; }

    public Transform EffectHolder => effectHolder;
    public Player Player { private set; get; }
    public DynamicJoystick DynamicJoystick => dynamicJoystick;


    public static bool IsState(GameState state) => gameState == state;

    protected void Awake()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
        UIManager.Ins.OpenUI<MainMenuUI>();
        ChangeState(GameState.MainMenu);
    }
    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.MainMenu:
                OnMainMenu();
                break;
            case GameState.GamePlay:
                OnStartGame();
                break;
            case GameState.Finish:
                OnFinish();
                break;
            case GameState.Setting:
                //tam thoi khong dung
                break;
            case GameState.Shop:
                OnShopping();
                break;
            default:
                break;
        }
    }

    private void OnMainMenu()
    {
        UIManager.Ins.OpenUI<MainMenuUI>();
        if (IsNewGame)
            PrepareLevel();
        CameraFollow.Ins.OnChangeOffSet(gameState);
        Player.ChangeAnim(Constants.ISIDLE_ANIM);
    }
    private void PrepareLevel()
    {
        AudioManager.Ins.PlayMusic(ESound.ThemeMusicOnMainMenu);
        Level = UserDataManager.Ins.GetLevel();
        LevelManager.Ins.OnLoadMap();
        Player = Spawner.Ins.GetPlayer();
        CameraFollow.Ins.FindCharacter(Player.CharacterVisual);
        LevelManager.Ins.CharacterOnPrepare();
    }
    private void OnStartGame()
    {
        CameraFollow.Ins.OnChangeOffSet(gameState);
        AudioManager.Ins.PlayMusic(ESound.ThemeMusicOnBattle);
        LevelManager.Ins.CharactersOnStartGame();
    }
    public void OnPlayAgain()
    {
        IsNewGame = true;
        ChangeState(GameState.MainMenu);
        LevelManager.Ins.CharactersOnEndGame();
    }
    private void OnFinish()
    {
        AudioManager.Ins.StopMusic();
        LevelManager.Ins.CharactersOnEndGame();
        CoinReceive = CalculateGoldReceive(Player.Score, LevelManager.Ins.GetCharacterRemain());
        int totalCoin = CoinReceive + UserDataManager.Ins.GetCoin();
        UserDataManager.Ins.SetCoin(totalCoin);
        UIManager.Ins.CloseUI<GamePlayUI>();
        // cho panel hiện ra sau 2-3s 
        if (!IsPlayerWin)
        {
            AudioManager.Ins.PlaySFX(ESound.Lose);
            UIManager.Ins.OpenUI<LoseUI>();
        }
        else
        {
            Player.ChangeAnim(Constants.ISWIN_ANIM);
            AudioManager.Ins.PlaySFX(ESound.Win);
            UIManager.Ins.OpenUI<WinUI>();
        }
    }
    public void OnNextLevel()
    {
        Level = Level += 1;
        if (Level >= LevelManager.Ins.TotalLevelNum)
        {
            IsMaxLevel = true;
            UserDataManager.Ins.SetMaxLevel(true);
            Level = 0;
        }
        UserDataManager.Ins.SetLevel(Level);
        IsNewGame = true;
        ChangeState(GameState.MainMenu);
    }
    private void OnShopping()
    {
        CameraFollow.Ins.OnChangeOffSet(gameState);
        Player.ChangeAnim(Constants.ISDANCE_ANIM);
    }
    public void IsPlayAgain(bool isPlayAgain) 
    {
        IsMaxLevel = !isPlayAgain;
        UserDataManager.Ins.SetMaxLevel(false);
    } 
    public Weapon GetWeapon(EWeaponType weaponType)
    {
       return weaponDataSO.GetWeapon((int)weaponType);
    }
    public Material GetColorMaterial(EColor colorEnum)
    {
        return colorDataSO.GetMaterials((int)colorEnum);
    }
    public Material GetPantMaterials(EPantType pantType)
    {
        return pantDataSO.GetMaterials((int)pantType);
    }
    public Hat GetHat(EHatType hatType)
    {
        return hatDataSO.GetHat((int)hatType);
    }

    public Weapon GetRandomWeapon()
    {
        EWeaponType randomWeapon = Utilities.RandomEnumValue<EWeaponType>();
        return GetWeapon(randomWeapon);
    }

    public Hat GetRandomHat()
    {
        EHatType randomWeapon = Utilities.RandomEnumValue<EHatType>();
        return GetHat(randomWeapon);
    }

    public Material GetRandomPant()
    {
        EPantType randomWeapon = Utilities.RandomEnumValue<EPantType>();
        return GetPantMaterials(randomWeapon);
    }

    public Material GetRandomColor()
    {
        EColor randomColor = Utilities.RandomEnumValue<EColor>();
        return GetColorMaterial(randomColor);
    }

    public string GetRandomName()
    {
        List<string> availableNames = characterNameDataSO.NameList.Where(name => !nameList.Contains(name)).ToList();

        if (availableNames.Count == 0)
        {
            Debug.Log("Không còn tên nào có sẵn");
            return "F";         
        }

        int id = UnityEngine.Random.Range(0, availableNames.Count);
        string charName = availableNames[id];

        nameList.Add(charName);
        return charName;
    }

    public void RemoveName(string characterName)
    {
        if (nameList.Contains(characterName))
            nameList.Remove(characterName);
    }

    public void RemoveAllName()
    {
        nameList.Clear();
    }

    public Effect GetEffect(EEffectType effectType)
    {
        return effectDataSO.GetEffect((int)effectType);
    }

    public int CalculateGoldReceive(float score, int rank )
    {
        int totalCharacters = LevelManager.Ins.CharacterNumbOfThisLevel;
        float percentage = 30f * (totalCharacters - rank) / totalCharacters;
        rank = (int)(rank <= 5 ? 50 : percentage);
        float total = score / 2 + rank;
        return Mathf.RoundToInt(total);
    }
}
