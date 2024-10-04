
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : UICanvas
{
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private Text levelText;
    [SerializeField] private Text tapToPlay;
    private void OnEnable()
    {
        levelText.text = "Level: " + (GameManager.Ins.Level + 1).ToString();
        if (GameManager.Ins.IsMaxLevel)
        {
            gameOverPanel.gameObject.SetActive(true);
        }
        tapToPlay.rectTransform.DOScale(1.3f, 1f).
                        SetEase(Ease.InOutSine).
                        SetLoops(-1, LoopType.Yoyo);
    }


    public void PlayAgain()
    {
        gameOverPanel.gameObject.SetActive(false);
        GameManager.Ins.IsPlayAgain(true);
    }
    public void PlayButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        GameManager.Ins.ChangeState(GameState.GamePlay);
        UIManager.Ins.OpenUI<GamePlay>();
        Close(0);
    }
    public void SettingButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        UIManager.Ins.OpenUI<Setting>();
        GameManager.Ins.ChangeState(GameState.Setting);
    }
}
