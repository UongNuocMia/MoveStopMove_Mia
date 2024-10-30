
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUI : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private RectTransform gameOverPanel, logoRect, shopButtonRect, settingButtonRect, tapToPlayRect, coinRect;
    [SerializeField] private RectTransform shopPosOnScreen, settingPosOnScreen, ttpPos, coinPos;
    private float defaultLogoSize = 1;
    [SerializeField] private GameObject mask;
    private Tween tapToPlayTween;

    private void OnEnable()
    {
        if (GameManager.Ins.IsMaxLevel)
        {
            gameOverPanel.gameObject.SetActive(true);
        }
        OnEnalbeAnim();
    }

    public void PlayAgain()
    {
        gameOverPanel.gameObject.SetActive(false);
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        GameManager.Ins.IsPlayAgain(true);
    }
    public void PlayButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        OnDisableAnim();
        Close(0.3f);
        Invoke(nameof(PlayGame), 0.3f);
    }
    private void PlayGame()
    {
        GameManager.Ins.ChangeState(GameState.GamePlay);
        UIManager.Ins.OpenUI<GamePlayUI>();
    }
    public void SettingButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        UIManager.Ins.OpenUI<SettingUI>();
    }  
    public void ShopButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        GameManager.Ins.ChangeState(GameState.Shop);
        OnDisableAnim();
        Invoke(nameof(OpenShopUI), 0.3f);
    }

    private void OpenShopUI()
    {
        UIManager.Ins.OpenUI<ShopUI>();
        Close(0.3f);
    }

    private void OnEnalbeAnim()
    {
        if (tapToPlayTween == null)
        tapToPlayTween = tapToPlayRect.DOScale(1.3f, 1f).
                    SetEase(Ease.InOutSine).
                    SetLoops(-1, LoopType.Yoyo);

        tapToPlayRect.DOMove(ttpPos.position, Constants.ANIM_DURATION);
        coinText.SetText($"{UserDataManager.Ins.GetCoin()}");
        coinRect.DOMove(coinPos.position, Constants.ANIM_DURATION);
        shopButtonRect.DOMove(shopPosOnScreen.position, Constants.ANIM_DURATION).SetEase(Ease.InOutSine);
        settingButtonRect.DOMove(settingPosOnScreen.position, Constants.ANIM_DURATION).SetEase(Ease.InOutSine);
        logoRect.DOScale(defaultLogoSize, Constants.ANIM_DURATION).SetEase(Ease.InOutSine).OnComplete(() => {
            mask.SetActive(false);
        });
    }

    private void OnDisableAnim()
    {
        mask.SetActive(true);
        tapToPlayRect.DOMove(new Vector3(tapToPlayRect.position.x, tapToPlayRect.position.y - 1000), Constants.ANIM_DURATION);
        coinRect.DOMove(new Vector3(coinRect.position.x + 1000, coinRect.position.y), Constants.ANIM_DURATION);
        shopButtonRect.DOMove(new Vector3(shopButtonRect.position.x - 2000, shopButtonRect.position.y), Constants.ANIM_DURATION).SetEase(Ease.InOutSine);
        settingButtonRect.DOMove(new Vector3(settingButtonRect.position.x + 2000, settingButtonRect.position.y), Constants.ANIM_DURATION).SetEase(Ease.InOutSine);
        logoRect.DOScale(0, Constants.ANIM_DURATION).SetEase(Ease.InOutSine);
    }
}
