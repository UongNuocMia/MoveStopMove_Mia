using UnityEngine;

public class UserData : Singleton<UserData>
{
    private string COIN_KEY = "PlayerCoinKey";
    private string lEVEL_KEY = "PlayerLevelKey";
    private string WEAPON_KEY = "PlayerWeaponKey";
    private string PANT_KEY = "PlayerPantKey";
    private string HEAD_KEY = "PlayerHeadKey";

    public int Level;

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(lEVEL_KEY, level);
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(lEVEL_KEY);
    }

    public void SetWeapon(EWeaponType weaponType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)weaponType);
    }

    public int GetWeapon()
    {
       return PlayerPrefs.GetInt(WEAPON_KEY,(int)EWeaponType.Hammer); //Change to candy
    }
    public void SetPant(EPantType pantType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)pantType);
    }
    public int GetPant()
    {
        return PlayerPrefs.GetInt(PANT_KEY, (int)EPantType.Chambi); // change to None
    }
    public void SetHead(EHeadType headType)
    {
        PlayerPrefs.SetInt(HEAD_KEY, (int)headType);
    }
    public int GetHead()
    {
        return PlayerPrefs.GetInt(HEAD_KEY, (int)EHeadType.Rau); // change to None
    }

    public void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(COIN_KEY, coin);
    }

    public int GetCoin()
    {
        return PlayerPrefs.GetInt(COIN_KEY, 0);
    }
}
