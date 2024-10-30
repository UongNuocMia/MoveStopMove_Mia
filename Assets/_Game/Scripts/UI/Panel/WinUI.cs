using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WinUI : UICanvas
{
    [SerializeField] private Button continueButton;
    [SerializeField] private RectTransform touchToCotinue, backgroundPanel;
    [SerializeField] private TextMeshProUGUI coinText;
    private Effect winEff;
    private float defaultSize = 1;

    private void OnEnable()
    {
        Init();
        AnimSetup();

        //winEff = GameManager.Ins.GetEffect(EEffectType.Win_VFX);
        //ParticlePool.Play(winEff.ThisEffect,
        //    new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -13.5f),
        //    Quaternion.identity);
    }

    private void OnDisable()
    {
        backgroundPanel.localScale = new Vector3(backgroundPanel.localScale.x, 0);
    }

    private void Init()
    {
        backgroundPanel.localScale = new Vector3(backgroundPanel.localScale.x, 0);

        coinText.SetText($"{GameManager.Ins.CoinReceive}");
        continueButton.onClick.AddListener(ContinueButton);

    }
    public void ContinueButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        GameManager.Ins.OnNextLevel();
        Close(0);
    }

    private void AnimSetup()
    {
        touchToCotinue.DOScale(1.3f, 1f).
                SetEase(Ease.InOutSine).
                SetLoops(-1, LoopType.Yoyo);

        backgroundPanel.DOScale(new Vector3(backgroundPanel.localScale.x, defaultSize), 0.5f).OnComplete(OnComplete);
    }

    private void OnComplete()
    {
        winEff = Instantiate(GameManager.Ins.GetEffect(EEffectType.Win_VFX), GameManager.Ins.EffectHolder);
    }
}
