using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UICanvas
{
    [SerializeField] private List<Sprite> soundSpriteList; // 0 - mute, 1 - not mute
    [SerializeField] private List<Sprite> musicSpriteList;
    [SerializeField] private Image musicImage;
    [SerializeField] private Image soundImage;
    [SerializeField] private Slider musicSlider, soundSlider;
    [SerializeField] private Toggle vibrationToggle;

    private bool isMuteSound;
    private bool isMuteMusic;

    private void OnEnable()
    {
        soundSlider.value = UserDataManager.Ins.GetSFXVolume();
        musicSlider.value = UserDataManager.Ins.GetMusicVolume();

        isMuteSound = UserDataManager.Ins.GetSFXVolume() > 0 ? false : true;
        isMuteMusic = UserDataManager.Ins.GetMusicVolume() > 0 ? false : true;

        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];
        musicImage.sprite = isMuteMusic ? musicSpriteList[0] : musicSpriteList[1];

        vibrationToggle.isOn = UserDataManager.Ins.GetVibration();

        vibrationToggle.onValueChanged.AddListener(OnSwitchVibrationToggle);
    }

    public void OnSwitchVibrationToggle(bool isOn)
    {
        UserDataManager.Ins.SetVibration(vibrationToggle.isOn);
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
        isMuteSound = soundSlider.value == 0 ? true : false;
        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];
        AudioManager.Ins.SetSFXVolume(soundSlider.value);
        UserDataManager.Ins.SetSFXVolume(soundSlider.value);
    }
    public void MusicVolume()
    {
        isMuteMusic = musicSlider.value == 0 ? true : false;
        musicImage.sprite = isMuteMusic ? musicSpriteList[0] : musicSpriteList[1];
        AudioManager.Ins.SetMusicVolume(musicSlider.value);
        UserDataManager.Ins.SetMusicVolume(musicSlider.value);
    }
    public void VibrationToggle()
    {
        vibrationToggle.isOn = !vibrationToggle.isOn;
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
        Close(0);
    }
}
