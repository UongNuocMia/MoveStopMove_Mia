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
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Player player;
    public Character Winner;
    public DynamicJoystick DynamicJoystick => dynamicJoystick;

    public int Level { private set; get; }
    public int PlayerScore { private set; get; }
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
                //PrepareLevel();
                break;
            case GameState.GamePlay:
                //OnStartGame();
                break;
            case GameState.Finish:
                OnFinish();
                break;
            case GameState.Setting:
                OnSetting();
                break;
            default:
                break;
        }
    }

    private void PrepareLevel()
    {
        Level = UserData.Ins.GetLevel();
        UIManager.Ins.OpenUI<MainMenu>();
        LevelManager.Ins.OnLoadMap();
        player = Spawner.Ins.GetPlayer();
        CameraFollow.FindCharacter(player.TF);
        LevelManager.Ins.CharacterOnPrepare();
    }
    private void OnStartGame()
    {
        LevelManager.Ins.CharactersOnStartGame();
    }
    public void OnPlayAgain()
    {
        ChangeState(GameState.MainMenu);
        LevelManager.Ins.CharactersOnEndGame();
    }

    private void OnSetting()
    {
        LevelManager.Ins.CharactersOnSetting();
    }

    private void OnFinish()
    {
        LevelManager.Ins.CharactersOnEndGame();
        List<Character> top3Characters = LevelManager.Ins.GetTop3Characters();
        PlayerScore = player.Score;
        UIManager.Ins.CloseUI<GamePlay>();
        if (Winner is Bot)
        {
            UIManager.Ins.OpenUI<Lose>();
        }
        else
        {
            UIManager.Ins.OpenUI<Win>();
        }
        //for (int i = 0; i < top3Characters.Count; i++)
        //{
        //    top3Characters[i].OnResult(LevelManager.Ins.RankTransformList[i], i);
        //}
        CameraFollow.FindCharacter(top3Characters[0].TF);
    }

    public void OnNextLevel()
    {
        Level = Level += 1;
        if (Level >= LevelManager.Ins.totalLevelNumb)
        {
            IsMaxLevel = true;
            Level = 0;
        }
        UserData.Ins.SetLevel(Level);
        ChangeState(GameState.MainMenu);
    }

    public void IsPlayAgain(bool isPlayAgain) => IsMaxLevel = !isPlayAgain;


    public Weapon GetWeapon(WeaponType weaponType)
    {
       return weaponDataSO.GetWeapon((int)weaponType);
    }

    public Material GetColorMaterial(ColorEnum colorEnum)
    {
        return colorDataSO.GetMaterials((int)colorEnum);
    }
}
