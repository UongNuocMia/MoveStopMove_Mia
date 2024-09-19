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

    public void SetWeapon(WeaponType weaponType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)weaponType);
    }

    public int GetWeapon()
    {
       return PlayerPrefs.GetInt(WEAPON_KEY,(int)WeaponType.Hammer); //Change to candy
    }
    public void SetPant(PantType pantType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)pantType);
    }
    public int GetPant()
    {
        return PlayerPrefs.GetInt(PANT_KEY, (int)PantType.Chambi); // change to None
    }
    public void SetHead(HeadType headType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)headType);
    }
    public int GetHead()
    {
        return PlayerPrefs.GetInt(PANT_KEY, (int)HeadType.Rau); // change to None
    }
}
