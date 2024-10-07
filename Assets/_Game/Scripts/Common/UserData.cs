using UnityEngine;

public class UserData : Singleton<UserData>
{
    private string COIN_KEY = "PlayerCoinKey";
    private string lEVEL_KEY = "PlayerLevelKey";
    private string WEAPON_KEY = "PlayerWeaponKey";
    private string PANT_KEY = "PlayerPantKey";
    private string HEAD_KEY = "PlayerHeadKey";
    private string SFX_KEY = "PlayerSoundKey";
    private string MUSIC_KEY = "PlayerMusicKey";
    private string VIBRATION_KEY = "PlayerVibrationKey";

    public bool IsHaveThisItem;

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(lEVEL_KEY, level);
    }
    public int GetLevel()=> PlayerPrefs.GetInt(lEVEL_KEY);

    public void SetWeapon(EWeaponType weaponType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)weaponType);
    }
    public int GetWeapon() => PlayerPrefs.GetInt(WEAPON_KEY, (int)EWeaponType.Hammer); //Change to candy 

    public void SetPant(EPantType pantType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)pantType);
    }
    public int GetPant()=> PlayerPrefs.GetInt(PANT_KEY, (int)EPantType.Chambi); // change to None
    
    public void SetHead(EHeadType headType)
    {
        PlayerPrefs.SetInt(HEAD_KEY, (int)headType);
    }
    public int GetHead() => PlayerPrefs.GetInt(HEAD_KEY, (int)EHeadType.Rau); // change to None    

    public void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(COIN_KEY, coin);
    }
    public int GetCoin() => PlayerPrefs.GetInt(COIN_KEY, 0);

    public void SetSFXVolume(float soundVolume)
    {
        PlayerPrefs.SetFloat(SFX_KEY, soundVolume);
    }
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_KEY, 1);

    public void SetMusicVolume(float musicVolume)
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, musicVolume);
    }
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_KEY, 1);

    public void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt(VIBRATION_KEY, isOn ? 1 : 0);
    }

    public bool GetVibration()
    {
        bool isOn = PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1 ? true : false;
        return isOn;
    }
}
