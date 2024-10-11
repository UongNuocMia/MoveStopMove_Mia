using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Setting, Shop }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private MaterialsDataSO colorDataSO;
    [SerializeField] private MaterialsDataSO pantDataSO;
    [SerializeField] private HatDataSO hatDataSO;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Player player;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;

    public int Level { private set; get; }
    public int PlayerScore { private set; get; }
    public bool IsPlayerWin = true;
    public bool IsMaxLevel { private set; get; }

    private static GameState gameState = GameState.MainMenu;
    public static bool IsState(GameState state) => gameState == state;

    public bool IsNewGame = true;

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
        AudioManager.Ins.PlayMusic(ESound.ThemeMusicOnMainMenu);
        if (IsNewGame)
            PrepareLevel();
        CameraFollow.Ins.OnChangeOffSet(gameState);
        player.ChangeAnim(Constants.ISIDLE_ANIM);
    }

    private void PrepareLevel()
    {
        Level = 3;//UserData.Ins.GetLevel();
        LevelManager.Ins.OnLoadMap();
        player = Spawner.Ins.GetPlayer();
        CameraFollow.Ins.FindCharacter(player.TF);
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
        PlayerScore = player.Score;
        UIManager.Ins.CloseUI<GamePlayUI>();
        // cho panel hiện ra sau 2-3s 
        if (!IsPlayerWin)
        {
            AudioManager.Ins.PlaySFX(ESound.Lose);
            UIManager.Ins.OpenUI<LoseUI>();
        }
        else
        {
            player.ChangeAnim(Constants.ISWIN_ANIM);
            AudioManager.Ins.PlaySFX(ESound.Win);
            UIManager.Ins.OpenUI<WinUI>();
        }
    }
    public void OnNextLevel()
    {
        Level = 3;
        Level = Level += 1;
        if (Level >= LevelManager.Ins.TotalLevelNum)
        {
            IsMaxLevel = true;
            Level = 0;
        }
        UserDataManager.Ins.SetLevel(Level);
        IsNewGame = true;
        ChangeState(GameState.MainMenu);
    }
    private void OnShopping()
    {
        CameraFollow.Ins.OnChangeOffSet(gameState);
        player.ChangeAnim(Constants.ISDANCE_ANIM);
    }
    public void IsPlayAgain(bool isPlayAgain) => IsMaxLevel = !isPlayAgain;
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
    public GameObject GetHat(EHatType hatType)
    {
        return hatDataSO.GetHat((int)hatType);
    }
}
