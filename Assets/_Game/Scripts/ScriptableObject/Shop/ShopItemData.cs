using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ShopItemsData<T> where T: Enum
{
    public List<ShopItemData<T>> ItemDataList;
}

[Serializable]
public class ShopItemData<T> : ShopItemData where T : Enum
{
    public T type;

}

[Serializable]
public class ShopItemData
{
    public Sprite sprIcon;
    public int price;
    public EBuffType buffType;
    public float buffValue;
    public string buffDescription;

    public string GetBuffDescription()
    {
        return buffType switch
        {
            EBuffType.Range => $"{buffValue}% Range",
            EBuffType.AttackSpeed => $"{buffValue}% Attack Speed",
            EBuffType.MoveSpeed => $"{buffValue}% Move Speed",
            _ => "",
        };
    }
}
