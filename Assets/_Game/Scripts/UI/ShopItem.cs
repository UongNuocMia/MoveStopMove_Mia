using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : GameUnit
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject lockGO, equippedGO, selectGO;
    private int price;
    private string buffDescription;

    private Enum type;

    public event Action<OnItemClickEventArgs> OnItemClick;
    public class OnItemClickEventArgs : EventArgs
    {
        public int price;
        public string buffDescription;
        public Enum Type;
        public ShopItem shopItem;
    }

    public void Setup<T>(ShopItemData<T> itemData, Action<OnItemClickEventArgs> onChangeItem) where T : Enum
    {
        itemIcon.sprite = itemData.sprIcon;
        price = itemData.price;
        buffDescription = itemData.GetBuffDescription();
        type = itemData.type;
        OnItemClick += onChangeItem;
        SetStatus();
    }

    public void SetStatus()
    {
        bool isEquipped = false;
        bool isPurchased = false;

        if(type is EWeaponType weaponType)
        {
            isEquipped = UserDataManager.Ins.GetWeapon() == weaponType;
            isPurchased = UserDataManager.Ins.GetPurchaseWeaponList().Contains(weaponType);
        }
        else if (type is EPantType pantType)
        {
            isEquipped = UserDataManager.Ins.GetPant() == pantType;
            isPurchased = UserDataManager.Ins.GetPurchasePantList().Contains(pantType);
        }
        else if (type is EHatType hatType)
        {
            isEquipped = UserDataManager.Ins.GetHat() == hatType;
            isPurchased = UserDataManager.Ins.GetPurchaseHatList().Contains(hatType);
        }
        lockGO.SetActive(!isEquipped && !isPurchased);
        equippedGO.SetActive(isEquipped);
        OnSelectThisItem(false);
    }


    public void OnShopItemClick()
    {
        OnItemClick?.Invoke(new OnItemClickEventArgs
        {
            price = price,
            buffDescription = buffDescription,
            Type = type,
            shopItem = this,
        });
    }

    public void OnEquipItem(bool isEquipped) => equippedGO.SetActive(isEquipped);
    public void OnSelectThisItem(bool isClick) => selectGO.SetActive(isClick);
    public void OnHideItem(bool isHide) => gameObject.SetActive(!isHide);

}
