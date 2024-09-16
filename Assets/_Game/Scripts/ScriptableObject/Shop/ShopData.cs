using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : ScriptableObject
{
    public ShopItemsData<PantType> panstData;
    public ShopItemsData<WeaponType> weaponData;

    public ShopItemData<PantType> GetPantData(PantType type)
    {
        for (int i = 0; i < panstData.itemsData.Count; i++)
        {
            if (panstData.itemsData[i].type == type) return panstData.itemsData[i];
        }
        return null;
    }
}
