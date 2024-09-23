using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/ShopData")]
public class ShopData : ScriptableObject
{
    public ShopItemsData<EPantType> panstData;
    public ShopItemsData<EWeaponType> weaponData;

    public ShopItemData<EPantType> GetPantData(EPantType type)
    {
        for (int i = 0; i < panstData.itemsData.Count; i++)
        {
            if (panstData.itemsData[i].type == type) return panstData.itemsData[i];
        }
        return null;
    }
}
