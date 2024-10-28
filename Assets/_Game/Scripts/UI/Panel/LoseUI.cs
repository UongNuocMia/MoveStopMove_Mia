using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LoseUI : UICanvas
{
    [SerializeField] private Text rankText;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI killerText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private RectTransform touchToCotinue, backgroundPanel;
    [SerializeField] private GameObject timeOutImageGO;


    private void OnEnable()
    {
        backgroundPanel.localScale = new Vector3(backgroundPanel.localScale.x, 0);
        Init();
        AnimSetup();
    }

    private void OnDisable()
    {
        backgroundPanel.localScale = new Vector3(backgroundPanel.localScale.x, 0);
    }
    private void ContinueButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        GameManager.Ins.IsNewGame = true;
        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }

    private void Init()
    {
        rankText.text = "Rank #" + LevelManager.Ins.GetCharacterRemain();

        coinText.SetText($"{GameManager.Ins.CoinReceive}");

        timeOutImageGO.SetActive(false);
 
        if (GameManager.Ins.Player.Killer == null && LevelManager.Ins.CurrentLevelType == ELevelType.Time)
        {
            killerText.SetText("");
            timeOutImageGO.SetActive(true);
        }
        else if (GameManager.Ins.Player.Killer != null)
        {
            string killerColorHex = ColorUtility.ToHtmlStringRGB(GameManager.Ins.Player.Killer.GetCharacterColor());
            string killerString = string.Format("You've been killed by\n<color=#{0}>{1}</color>", killerColorHex, GameManager.Ins.Player.Killer.CharacterName);
            killerText.SetText(killerString);
        }

        continueButton.onClick.AddListener(ContinueButton);
    }

    private void AnimSetup()
    {
        touchToCotinue.DOScale(1.3f, 1f).
                SetEase(Ease.InOutSine).
                SetLoops(-1, LoopType.Yoyo);

        backgroundPanel.DOScale(new Vector3(backgroundPanel.localScale.x, 1f), 0.5f);
    }
}