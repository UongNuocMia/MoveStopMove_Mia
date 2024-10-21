using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private static string SAVE_DATA_KEY = "SaveData_";
    private static string COIN_KEY = "PlayerCoinKey";
    private static string lEVEL_KEY = "PlayerLevelKey";
    private static string WEAPON_KEY = "PlayerWeaponKey";
    private static string PANT_KEY = "PlayerPantKey";
    private static string HAT_KEY = "PlayerHatKey";
    private static string SFX_KEY = "PlayerSoundKey";
    private static string MUSIC_KEY = "PlayerMusicKey";
    private static string VIBRATION_KEY = "PlayerVibrationKey";

    private SaveItemShopData saveItemHatData;

    private void Awake()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(SAVE_DATA_KEY)))
        {
            saveItemHatData = new();
        }
        else
        {
            saveItemHatData = JsonUtility.FromJson<SaveItemShopData>(PlayerPrefs.GetString(SAVE_DATA_KEY));
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
    public EWeaponType GetWeapon()
    {
        EWeaponType weaponType = (EWeaponType)PlayerPrefs.GetInt(WEAPON_KEY, (int)EWeaponType.Hammer);
        return weaponType;
    }

    public void SetPant(EPantType pantType)
    {
        PlayerPrefs.SetInt(PANT_KEY, (int)pantType);
    }
    public EPantType GetPant()
    {
        EPantType pantType = (EPantType)PlayerPrefs.GetInt(PANT_KEY, (int)EPantType.None);
        return pantType;
    }
    
    public void SetHat(EHatType hatType)
    {
        PlayerPrefs.SetInt(HAT_KEY, (int)hatType);
    }
    public EHatType GetHat()
    {
        EHatType hatType = (EHatType)PlayerPrefs.GetInt(HAT_KEY, (int)EHatType.None);
        return hatType;
    }   

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

    public void SetPurchaseWeapon(EWeaponType weaponType)
    {
        if (saveItemHatData.purchaseWeapon.Contains(weaponType)) return;
        saveItemHatData.purchaseWeapon.Add(weaponType);
    }

    public void SetPurchasePant(EPantType pantType)
    {
        if (saveItemHatData.purchasePants.Contains(pantType)) return;
        saveItemHatData.purchasePants.Add(pantType);
    }

    public List<EHatType> GetPurchaseHatList()
    {
        return saveItemHatData.purchaseHats;
    }

    public List<EPantType> GetPurchasePantList()
    {
        return saveItemHatData.purchasePants;
    }

    public List<EWeaponType> GetPurchaseWeaponList()
    {
        return saveItemHatData.purchaseWeapon;
    }

    public void SaveData()
    {
        string userData = JsonUtility.ToJson(saveItemHatData);
        PlayerPrefs.SetString(SAVE_DATA_KEY, userData);
    }
}

[Serializable]

public class SaveItemShopData
{
    public List<EHatType> purchaseHats;
    public List<EPantType> purchasePants;
    public List<EWeaponType> purchaseWeapon;

    public SaveItemShopData()
    {
        purchaseHats = new()
        {
            EHatType.None
        };

        purchasePants = new()
        {
            EPantType.None
        };

        purchaseWeapon = new()
        {
            EWeaponType.Hammer
        };
    }
}
