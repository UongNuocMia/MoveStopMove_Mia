using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LoseUI : UICanvas
{
    [SerializeField] private Text rankText;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI killerText;


    private void OnEnable()
    {
        continueButton.onClick.AddListener(ContinueButton);
        SetText();
    }
    private void ContinueButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        UIManager.Ins.OpenUI<MainMenuUI>();
        GameManager.Ins.IsNewGame = true;
        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }

    private void SetText()
    {
        rankText.text = "Rank #" + LevelManager.Ins.GetCharacterRemain();
        if (GameManager.Ins.Player.Killer == null && LevelManager.Ins.CurrentLevelType == ELevelType.Time)
            killerText.SetText("Time out");
        else if(GameManager.Ins.Player.Killer != null)
        {
            string killerColorHex = ColorUtility.ToHtmlStringRGB(GameManager.Ins.Player.Killer.GetCharacterColor());
            string killerString = string.Format("You've been killed by <color=#{0}>{1}</color>", killerColorHex, GameManager.Ins.Player.Killer.CharacterName);
            killerText.SetText(killerString);
        }
    }
}