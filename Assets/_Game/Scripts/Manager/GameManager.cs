using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { MainMenu, GamePlay, Finish, Setting }

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    [SerializeField] private WeaponDataSO weaponDataSO;
    [SerializeField] private MaterialsDataSO colorDataSO;
    [SerializeField] private MaterialsDataSO pantDataSO;
    [SerializeField] private HeadDataSO headDataSO;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Player player;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;

    public int Level { private set; get; }
    public int PlayerScore { private set; get; }
    public bool IsPlayerWin = true;
    public bool IsMaxLevel { private set; get; }

    private static GameState gameState = GameState.MainMenu;
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

        //csv.OnInit();
        //userData?.OnInitData();
        UIManager.Ins.OpenUI<MainMenu>();
        ChangeState(GameState.MainMenu);
        //CameraFollow.FindCharacter(player.transform);

    }
    public void ChangeState(GameState state)
    {
        gameState = state;
        switch (state)
        {
            case GameState.MainMenu:
                PrepareLevel();
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
            default:
                break;
        }
    }

    private void PrepareLevel()
    {
        Level = 3;//UserData.Ins.GetLevel();
        UIManager.Ins.OpenUI<MainMenu>();
        LevelManager.Ins.OnLoadMap();
        player = Spawner.Ins.GetPlayer();
        CameraFollow.FindCharacter(player.TF);
        LevelManager.Ins.CharacterOnPrepare();
        AudioManager.Ins.PlayMusic(ESound.ThemeMusicOnMainMenu);
    }
    private void OnStartGame()
    {
        AudioManager.Ins.PlayMusic(ESound.ThemeMusicOnBattle);
        LevelManager.Ins.CharactersOnStartGame();
    }
    public void OnPlayAgain()
    {
        ChangeState(GameState.MainMenu);
        LevelManager.Ins.CharactersOnEndGame();
    }

    private void OnFinish()
    {
        AudioManager.Ins.StopMusic();

        LevelManager.Ins.CharactersOnEndGame();
        PlayerScore = player.Score;
        UIManager.Ins.CloseUI<GamePlay>();
        // cho panel hiện ra sau 2-3s 
        if (!IsPlayerWin)
        {
            AudioManager.Ins.PlaySFX(ESound.Lose);
            UIManager.Ins.OpenUI<Lose>();
        }
        else
        {
            AudioManager.Ins.PlaySFX(ESound.Win);
            UIManager.Ins.OpenUI<Win>();
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
        UserData.Ins.SetLevel(Level);
        ChangeState(GameState.MainMenu);
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

    public GameObject GetHead(EHeadType headType)
    {
        return headDataSO.GetHead((int)headType);
    }
}
