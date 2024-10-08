using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class ShopItem : GameUnit
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject lockGO, equippedGO;
    private float price;
    private string buffDescription;

    public event EventHandler<OnItemClickEventArgs> OnItemClick;
    public class OnItemClickEventArgs : EventArgs
    {
        public float price;
        public string buffDescription;
    }


    private void OnInit()
    {
        lockGO.SetActive(true);
        if (UserData.Ins.IsHaveThisItem)
        {
            lockGO.SetActive(false);
        }
       //if() UserData.Ins.GetWeapon()
    }

    public void SetData(Sprite icon, float price, string buffDescription)
    {
        itemIcon.sprite = icon;
        this.price = price;
        this.buffDescription = buffDescription;
    }

    public void OnShopItemClick()
    {
        OnItemClick?.Invoke(this, new OnItemClickEventArgs
        {
            price = this.price,
            buffDescription = this.buffDescription
        });
    }
}
