using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private static string SAVE_DATA_KEY = "SaveData";
    private static string COIN_KEY = "PlayerCoinKey";
    private static string lEVEL_KEY = "PlayerLevelKey";
    private static string WEAPON_KEY = "PlayerWeaponKey";
    private static string PANT_KEY = "PlayerPantKey";
    private static string HAT_KEY = "PlayerHatKey";
    private static string SFX_KEY = "PlayerSoundKey";
    private static string MUSIC_KEY = "PlayerMusicKey";
    private static string VIBRATION_KEY = "PlayerVibrationKey";

    private SaveItemHatData saveItemHatData;

    private void Awake()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(SAVE_DATA_KEY)))
        {
            saveItemHatData = new();
        }
        else
        {
            saveItemHatData = JsonUtility.FromJson<SaveItemHatData>(PlayerPrefs.GetString(SAVE_DATA_KEY));
        }
    }

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
    
    public void SetHat(EHatType hatType)
    {
        PlayerPrefs.SetInt(HAT_KEY, (int)hatType);
    }
    public int GetHat() => PlayerPrefs.GetInt(HAT_KEY, (int)EHatType.Rau); // change to None    

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
        bool isOn = PlayerPrefs.GetInt(VIBRATION_KEY, 1) == 1;
        return isOn;
    }

    public void SetPurchaseHat(EHatType hatType)
    {
        if (saveItemHatData.purchaseHats.Contains(hatType)) return;
        saveItemHatData.purchaseHats.Add(hatType);
    }

    public void SaveData()
    {
        string userData = JsonUtility.ToJson(saveItemHatData);
        PlayerPrefs.SetString(SAVE_DATA_KEY, userData);
    }
}

public class SaveItemHatData
{
    public List<EHatType> purchaseHats;

    public SaveItemHatData()
    {
        purchaseHats = new()
        {
            EHatType.None
        };
    }
}
