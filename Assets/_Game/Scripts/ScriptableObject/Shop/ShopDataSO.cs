using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/ShopData")]
public class ShopDataSO : ScriptableObject
{
    public ShopItemsData<EPantType> PantData;
    public ShopItemsData<EWeaponType> WeaponData;
    public ShopItemsData<EHatType> HatData;

    public ShopItemData<EPantType> GetPantData(EPantType type)
    {
        for (int i = 0; i < PantData.ItemDataList.Count; i++)
        {
            if (PantData.ItemDataList[i].type == type) return PantData.ItemDataList[i];
        }
        return null;
    }

}
