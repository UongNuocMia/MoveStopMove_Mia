using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UICanvas
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text buffDescText;
    [SerializeField] private Text priceText;
    [SerializeField] private ShopDataSO shopData;
    [SerializeField] private Transform contentTF;
    [SerializeField] private List<Tab> tabList;

    private List<ShopItem> currentItemList = new();

    private void OnEnable()
    {
        SetCoin();
        OnTabChange(2);
        currentItemList[0].OnShopItemClick();
    }

    public void OnTabChange(int tabID)
    {
        currentItemList.Clear();
        switch (tabID)
        {
            case 0:
                for (int i = 0; i < shopData.WeaponData.itemsData.Count; i++)
                {
                    ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                    shopItem.SetData(shopData.WeaponData.itemsData[i].sprIcon,
                                     shopData.WeaponData.itemsData[i].price,
                                     shopData.WeaponData.itemsData[i].GetBuffDescription());
                    shopItem.OnItemClick += OnChangeItem;
                    currentItemList.Add(shopItem);
                }
                break;
            case 1:
                for (int i = 0; i < shopData.PanstData.itemsData.Count; i++)
                {
                    ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                    shopItem.SetData(shopData.PanstData.itemsData[i].sprIcon,
                                     shopData.PanstData.itemsData[i].price,
                                     shopData.PanstData.itemsData[i].GetBuffDescription());
                    shopItem.OnItemClick += OnChangeItem;
                    currentItemList.Add(shopItem);
                }
                break;
            case 2:
                for (int i = 0; i < shopData.HeadData.itemsData.Count; i++)
                {
                    ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                    shopItem.SetData(shopData.HeadData.itemsData[i].sprIcon,
                                     shopData.HeadData.itemsData[i].price,
                                     shopData.HeadData.itemsData[i].GetBuffDescription());
                    shopItem.OnItemClick += OnChangeItem;
                    currentItemList.Add(shopItem);
                }
                break;
            default:
                break;
        }
        for (int i = 0; i < tabList.Count; i++)
        {
            if (i == tabID)
                tabList[tabID].OnActivityTab();
            else
                tabList[i].OnDeActivityTab();
        }

    }

    private void OnChangeItem(object sender, ShopItem.OnItemClickEventArgs e)
    {
        priceText.text = e.price.ToString();
        buffDescText.text = e.buffDescription;
    }

    private void OnClickEquip()
    {

    }

    private void OnClickBuy()
    {

    }

    private void SetCoin()
    {
        coinText.text = UserData.Ins.GetCoin().ToString();
    }

    public void OnExitClick()
    {
        Close(0);
    }
}
