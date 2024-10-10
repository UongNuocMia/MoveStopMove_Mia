using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UICanvas
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text buffDescText;
    [SerializeField] private Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private ShopDataSO shopData;
    [SerializeField] private Transform contentTF;
    [SerializeField] private List<Tab> tabList;

    private List<ShopItem> currentItemList = new();

    private void OnEnable()
    {
        SetCoin();
        OnTabChange(0);
        currentItemList[0].OnShopItemClick();
        buyButton.onClick.AddListener(OnBuyClick);
        equipButton.onClick.AddListener(OnEquipClick);
    }

    public void OnTabChange(int tabID)
    {
        int shopItemCount = 0;
        switch (tabID)
        {
            case 0:
                for (int i = 0; i < shopData.WeaponData.itemsData.Count; i++)
                {
                    if (currentItemList.Count < shopData.WeaponData.itemsData.Count)
                    {
                        ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                        currentItemList.Add(shopItem);
                    }
                    currentItemList[i].SetData(shopData.WeaponData.itemsData[i].sprIcon,
                                     shopData.WeaponData.itemsData[i].price,
                                     shopData.WeaponData.itemsData[i].GetBuffDescription(), OnChangeItem);
                    currentItemList[i].SetEnum(EShopType.Weapon,shopData.WeaponData.itemsData[i].type);
                }
                shopItemCount = shopData.WeaponData.itemsData.Count;
                break;
            case 1:
                for (int i = 0; i < shopData.PanstData.itemsData.Count; i++)
                {
                    if (currentItemList.Count < shopData.PanstData.itemsData.Count)
                    {
                        ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                        currentItemList.Add(shopItem);
                    }
                    currentItemList[i].SetData(shopData.PanstData.itemsData[i].sprIcon,
                                     shopData.PanstData.itemsData[i].price,
                                     shopData.PanstData.itemsData[i].GetBuffDescription(), 
                                     OnChangeItem);
                    currentItemList[i].SetEnum(EShopType.Pants, EWeaponType.None, shopData.PanstData.itemsData[i].type);

                }
                shopItemCount = shopData.PanstData.itemsData.Count;

                break;
            case 2:
                for (int i = 0; i < shopData.HeadData.itemsData.Count; i++)
                {
                    if (currentItemList.Count < shopData.HeadData.itemsData.Count)
                    {
                        ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                        currentItemList.Add(shopItem);
                    }
                    currentItemList[i].SetData(shopData.HeadData.itemsData[i].sprIcon,
                                     shopData.HeadData.itemsData[i].price,
                                     shopData.HeadData.itemsData[i].GetBuffDescription(), OnChangeItem);
                    currentItemList[i].SetEnum(EShopType.Head, EWeaponType.None, EPantType.None, shopData.HeadData.itemsData[i].type);

                }
                shopItemCount = shopData.HeadData.itemsData.Count;
                break;
            default:
                break;
        }
        for (int i = 0; i < currentItemList.Count; i++)
        {
            currentItemList[i].OnHideItem(i > shopItemCount);
        }

        for (int i = 0; i < tabList.Count; i++)
        {
            if (i == tabID)
                tabList[tabID].OnActivityTab();
            else
                tabList[i].OnDeActivityTab();
        }
    }
    private void OnChangeItem(ShopItem.OnItemClickEventArgs args)
    {
        priceText.text = args.price.ToString();
        buffDescText.text = args.buffDescription;
        switch (args.shopType)
        {
            case EShopType.None:
                break;
            case EShopType.Weapon:
                if (UserData.Ins.GetWeapon() == (int)args.weaponType)
                {
                    equipButton.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                }
                break;
            case EShopType.Pants:
                if (UserData.Ins.GetPant() == (int)args.pantType)
                {
                    equipButton.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                }
                break;
            case EShopType.Head:
                if (UserData.Ins.GetHead() == (int)args.headType)
                {
                    equipButton.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    private void ChangeOnPlayer()
    {

    }
    private void OnEquipClick()
    {

    }

    private void OnBuyClick()
    {

    }

    private void SetCoin()
    {
        coinText.text = UserData.Ins.GetCoin().ToString();
    }

    public void OnExitClick()
    {
        GameManager.Ins.IsNewGame = false;
        GameManager.Ins.ChangeState(GameState.MainMenu);
        Close(0);
    }
}
