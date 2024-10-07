using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/ShopData")]
public class ShopData : ScriptableObject
{
    public ShopItemsData<EPantType> PanstData;
    public ShopItemsData<EWeaponType> WeaponData;
    public ShopItemsData<EHeadType> HeadData;

    public ShopItemData<EPantType> GetPantData(EPantType type)
    {
        for (int i = 0; i < PanstData.itemsData.Count; i++)
        {
            if (PanstData.itemsData[i].type == type) return PanstData.itemsData[i];
        }
        return null;
    }

}
