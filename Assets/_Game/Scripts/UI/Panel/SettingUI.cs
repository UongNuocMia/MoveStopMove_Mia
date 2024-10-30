using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UICanvas
{
    [SerializeField] private List<Sprite> soundSpriteList; // 0 - mute, 1 - not mute
    [SerializeField] private List<Sprite> musicSpriteList;
    [SerializeField] private Image musicImage, soundImage;
    [SerializeField] private Button musicButton, soundButton, vibrationButton, exitButton;
    [SerializeField] private Toggle vibrationToggle;
    [SerializeField] private Slider musicSlider, soundSlider;
    [SerializeField] private GameObject buttonGroup;

    [SerializeField] private RectTransform popupRect, popupPos;

    private bool isMuteSound;
    private bool isMuteMusic;

    private void OnEnable()
    {
        OnEnableAnim();
        Init();
    }
    private void Init()
    {
        soundSlider.value = UserDataManager.Ins.GetSFXVolume();
        musicSlider.value = UserDataManager.Ins.GetMusicVolume();
        isMuteSound = UserDataManager.Ins.GetSFXVolume() <= 0;
        isMuteMusic = UserDataManager.Ins.GetMusicVolume() <= 0;

        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];
        musicImage.sprite = isMuteMusic ? musicSpriteList[0] : musicSpriteList[1];

        vibrationToggle.isOn = UserDataManager.Ins.GetVibration();

        vibrationToggle.onValueChanged.AddListener(OnSwitchVibrationToggle);

        buttonGroup.SetActive(GameManager.IsState(GameState.GamePlay));

        exitButton.onClick.AddListener(CloseButton);
        musicButton.onClick.AddListener(OnMusicButtonClick);
        soundButton.onClick.AddListener(OnSoundButtonClick);
        vibrationButton.onClick.AddListener(VibrationToggle);

    }
    public void OnSoundButtonClick()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        isMuteSound = !isMuteSound;
        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];
        soundSlider.value = isMuteSound ? 0 : 1;
        UserDataManager.Ins.SetSFXVolume(soundSlider.value);
    }
    public void OnMusicButtonClick()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        isMuteMusic = !isMuteMusic;
        musicImage.sprite = isMuteMusic ? musicSpriteList[0] : musicSpriteList[1];
        musicSlider.value = isMuteMusic ? 0 : 1;
        UserDataManager.Ins.SetMusicVolume(musicSlider.value);

    }
    public void SFXVolume()
    {
        isMuteSound = soundSlider.value == 0;
        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];
        AudioManager.Ins.SetSFXVolume(soundSlider.value);
        UserDataManager.Ins.SetSFXVolume(soundSlider.value);
    }
    public void MusicVolume()
    {
        isMuteMusic = musicSlider.value == 0;
        musicImage.sprite = isMuteMusic ? musicSpriteList[0] : musicSpriteList[1];
        AudioManager.Ins.SetMusicVolume(musicSlider.value);
        UserDataManager.Ins.SetMusicVolume(musicSlider.value);
    }
    public void VibrationToggle()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        vibrationToggle.isOn = !vibrationToggle.isOn;
        UserDataManager.Ins.SetVibration(vibrationToggle.isOn);
    }
    public void OnSwitchVibrationToggle(bool isOn)
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        UserDataManager.Ins.SetVibration(vibrationToggle.isOn);
    }

    public void ContinueButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        Close(0);
    }
    public void TryAgainButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        GameManager.Ins.OnPlayAgain();
        UIManager.Ins.GetUI<GamePlayUI>().Close(0);
        Close(0);
    }

    private void CloseButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        OnDisableAnim();
        Close(0.3f);
    }

    private void OnEnableAnim()
    {
        popupRect.DOMoveY(popupPos.position.y, Constants.ANIM_DURATION);
    }

    private void OnDisableAnim() 
    {
        popupRect.DOMoveY(popupPos.position.y - 2000, Constants.ANIM_DURATION);
    }
}
