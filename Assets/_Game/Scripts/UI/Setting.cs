using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : UICanvas
{
    [SerializeField] private List<Sprite> soundSpriteList;
    [SerializeField] private List<Sprite> musicSpriteList;
    [SerializeField] private Image musicImage;
    [SerializeField] private Image soundImage;
    [SerializeField] private Slider musicSlider, soundSlider;

    private bool isMuteSound;
    private bool isMuteMusic;



    private void OnEnable()
    {
        isMuteSound = UserData.Ins.GetSFXVolume() > 0 ? false : true;
        isMuteMusic = UserData.Ins.GetMusicVolume() > 0 ? false : true;

        musicImage.sprite = isMuteSound ? musicSpriteList[0] : musicSpriteList[1];

        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];

        musicSlider.value = UserData.Ins.GetMusicVolume();
        soundSlider.value = UserData.Ins.GetMusicVolume();
    }

    public void OnClickSoundButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        isMuteSound = !isMuteSound;
        soundImage.sprite = isMuteSound ? soundSpriteList[0] : soundSpriteList[1];
        soundSlider.value = isMuteSound ? 0 : 1;
        UserData.Ins.SetSFXVolume(soundSlider.value);
    }
    public void OnClickMusicButton()
    {
        AudioManager.Ins.PlaySFX(ESound.ButtonClick);
        isMuteMusic = !isMuteMusic;
        musicImage.sprite = isMuteMusic ? musicSpriteList[0] : musicSpriteList[1];
        musicSlider.value = isMuteMusic ? 0 : 1;
        UserData.Ins.SetMusicVolume(musicSlider.value);

    }

    public void MusicVolume()
    {
        AudioManager.Ins.SetMusicVolume(musicSlider.value);
        UserData.Ins.SetMusicVolume(musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Ins.SetSFXVolume(soundSlider.value);
        UserData.Ins.SetSFXVolume(soundSlider.value);
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
