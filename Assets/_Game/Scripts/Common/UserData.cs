using UnityEngine;

public class UserData : Singleton<UserData>
{
    private string COIN_KEY = "PlayerCoinKey";
    private string lEVEL_KEY = "PlayerLevelKey";
    private string WEAPON_KEY = "PlayerWeaponKey";

    public int Level;

    public void SetLevel(int level)
    {
        PlayerPrefs.SetInt(lEVEL_KEY, level);
    }
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(lEVEL_KEY);
    }

    public void GetWeapon(WeaponType weaponType)
    {
        PlayerPrefs.SetInt(WEAPON_KEY, (int)weaponType);
    }
}
