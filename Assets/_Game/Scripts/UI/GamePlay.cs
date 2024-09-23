
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : UICanvas
{
    [SerializeField] private Text remainText;
    public void SettingButton()
    {
        UIManager.Ins.OpenUI<Setting>();
        GameManager.Ins.ChangeState(GameState.Setting);
    }
    private void OnEnable()
    {
        SetRemain();
    }

    public void SetRemain()
    {
        remainText.text = "Remain: " + LevelManager.Ins.GetCharacterRemain();
    }
}
