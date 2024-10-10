using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : GameUnit
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject lockGO, equippedGO;
    private float price;
    private string buffDescription;


    public EShopType shopType = EShopType.Weapon;
    public EHeadType headType { private get; set; } = EHeadType.None;
    public EPantType pantType { private get; set; } = EPantType.None; 
    public EWeaponType weaponType { private get; set; } = EWeaponType.None;

    public event Action<OnItemClickEventArgs> OnItemClick;
    public class OnItemClickEventArgs : EventArgs
    {
        public float price;
        public string buffDescription;
        public EShopType shopType = EShopType.Weapon;
        public EHeadType headType = EHeadType.None;
        public EPantType pantType = EPantType.None;
        public EWeaponType weaponType = EWeaponType.None;
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

    public void SetData(Sprite icon, float price, string buffDescription, Action<OnItemClickEventArgs> onChangeItem)
    {
        itemIcon.sprite = icon;
        this.price = price;
        this.buffDescription = buffDescription;
        OnItemClick += onChangeItem;
    }
    
    public void SetEnum(EShopType shopType, EWeaponType weaponType = EWeaponType.None, EPantType pantType = EPantType.None, EHeadType headType = EHeadType.None)
    {
        this.shopType = shopType;
        this.weaponType = weaponType;
        this.pantType = pantType;
        this.headType = headType;
    }

    public void OnShopItemClick()
    {
        OnItemClick?.Invoke(new OnItemClickEventArgs
        {
            price = price,
            buffDescription = buffDescription,
            headType = headType,
            pantType = pantType,
            weaponType = weaponType,
            shopType = shopType
        });
    }

    public void OnHideItem(bool isHide) => gameObject.SetActive(!isHide);
}
