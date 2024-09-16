using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ShopItemsData<T> where T: Enum
{
    public List<ShopItemData<T>> itemsData;
}
[Serializable]
public class ShopItemData<T> where T: Enum
{
    public Sprite sprIcon;
    public float price;
    public T type;
    public BuffType buffType;
    public float buffValue;
    public string buffDescription;
    public string GetBuffDescription()
    {
        switch (buffType)
        {
            case BuffType.Range:
                return $"{buffValue}% Range";
            case BuffType.AttackSpeed:
                return $"{buffValue}% Attack Speed";
            default:
                return "";
        }
    }
}
