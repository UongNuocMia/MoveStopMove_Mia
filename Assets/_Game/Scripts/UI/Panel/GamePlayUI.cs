
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : UICanvas
{
    [SerializeField] private Text remainCharacterText;
    [SerializeField] private Text remainTimeText;
    [SerializeField] private GameObject remainTimeGO;
    private float remainTime;
    public void SettingButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        UIManager.Ins.OpenUI<SettingUI>();
        GameManager.Ins.ChangeState(GameState.Setting);
    }
    private void OnEnable()
    {
        SetRemain();
        remainTimeGO.SetActive(false);
        if (LevelManager.Ins.CurrentLevelType == ELevelType.Time)
        {
            remainTimeGO.SetActive(true);
            remainTime = LevelManager.Ins.TimeRemain;
        }

    }

    private void Update()
    {
        if (LevelManager.Ins.CurrentLevelType == ELevelType.Time && GameManager.IsState(GameState.GamePlay))
        {
            remainTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainTime / 60);
            int seconds = Mathf.FloorToInt(remainTime % 60);
            remainTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (remainTime <= 0)
            {
                GameManager.Ins.IsPlayerWin = false;
                GameManager.Ins.ChangeState(GameState.Finish);
            }
        }
    }

    public void SetRemain()
    {
        remainCharacterText.text = "" + LevelManager.Ins.GetCharacterRemain();
    }
}
