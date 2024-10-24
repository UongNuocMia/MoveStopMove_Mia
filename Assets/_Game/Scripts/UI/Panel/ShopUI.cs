using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopUI : UICanvas
{
    [SerializeField] private Text buffDescText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button un_equipButton;
    [SerializeField] private Transform contentTF;
    [SerializeField] private List<TabUI> tabList;
    [SerializeField] private ShopDataSO shopData;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI priceText;

    private int price;
    private int currentCoin;
    private int currentTabID = -1;
    private int itemClickIndex = 0;
    private bool isRefresh;
    private EShopType shopType;
    private Enum type;
    private ShopItem currentShopItem;
    private List<ShopItem> currentItemList = new();
    private List<EHatType> hatTypeList;
    private List<EPantType> pantTypeList;
    private List<EWeaponType> weaponTypeList;

    private void OnEnable()
    {
        Init();
    }
    private void Init()
    {
        hatTypeList = UserDataManager.Ins.GetPurchaseHatList();
        pantTypeList = UserDataManager.Ins.GetPurchasePantList();
        weaponTypeList = UserDataManager.Ins.GetPurchaseWeaponList();
        SetCoin();
        OnTabChange(0);
        buyButton.onClick.AddListener(OnBuyClick);
        equipButton.onClick.AddListener(OnEquipClick);
        un_equipButton.onClick.AddListener(OnUnequipClick);
    }
    private void UpdateTabActivity(int activeTabID)
    {
        for (int i = 0; i < tabList.Count; i++)
        {
            if (i == activeTabID)
                tabList[i].OnActivityTab();
            else
                tabList[i].OnDeActivityTab();
        }
    }
    private void OnChangeItem(ShopItem.OnItemClickEventArgs args)
    {
        if (currentShopItem != null)
            currentShopItem.OnSelectThisItem(false);
        currentShopItem = args.shopItem;
        price = args.price;
        priceText.SetText($"{args.price}");
        buffDescText.text = args.buffDescription;
        type = args.Type;
        bool isEquipped = false;
        bool isInList = false;
        currentShopItem.OnSelectThisItem(true);

        switch (shopType)
        {
            case EShopType.Weapon:
                isEquipped = UserDataManager.Ins.GetWeapon() == (EWeaponType)type;
                isInList = weaponTypeList.Contains((EWeaponType)type);
                un_equipButton.gameObject.SetActive(false);
                break;
            case EShopType.Pant:
                isEquipped = UserDataManager.Ins.GetPant() == (EPantType)type;
                Debug.Log("getpant____" + UserDataManager.Ins.GetPant());
                isInList = pantTypeList.Contains((EPantType)type);
                un_equipButton.gameObject.SetActive(isEquipped);
                break;
            case EShopType.Hat:
                isEquipped = UserDataManager.Ins.GetHat() == (EHatType)type;
                isInList = hatTypeList.Contains((EHatType)type); 
                un_equipButton.gameObject.SetActive(isEquipped);
                break;
        }
        ChangeOnPlayer();
        equipButton.gameObject.SetActive(isInList && !isEquipped);
        buyButton.gameObject.SetActive(!isInList);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            UserDataManager.Ins.SetCoin(500);
        }
    }
    private void ChangeOnPlayer()
    {
        GameManager.Ins.Player.OnShopping(shopType, type);
    }
    private void OnEquipClick()
    {
        switch (shopType)
        {
            case EShopType.Weapon:
                UserDataManager.Ins.SetWeapon((EWeaponType)type);
                break;
            case EShopType.Pant:
                UserDataManager.Ins.SetPant((EPantType)type);
                break;
            case EShopType.Hat:
                UserDataManager.Ins.SetHat((EHatType)type);
                break;
        }
        RefreshTab();
    }
    private void OnUnequipClick()
    {
        switch (shopType)
        {
            case EShopType.Pant:
                type = EPantType.None;
                UserDataManager.Ins.SetPant(EPantType.None);
                break;
            case EShopType.Hat:
                type = EHatType.None;
                UserDataManager.Ins.SetHat(EHatType.None);
                break;
        }
        ChangeOnPlayer();
        currentShopItem.OnEquipItem(false);
        un_equipButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(true);
    }
    private void OnBuyClick()
    {
        if (currentCoin < price)
            return;
        currentCoin -= price;
        UserDataManager.Ins.SetCoin(currentCoin);
        SetCoin();
        switch (shopType)
        {
            case EShopType.Weapon:
                UserDataManager.Ins.SetPurchaseWeapon((EWeaponType)type);
                break;
            case EShopType.Pant:
                UserDataManager.Ins.SetPurchasePant((EPantType)type);
                break;
            case EShopType.Hat:
                UserDataManager.Ins.SetPurchaseHat((EHatType)type);
                break;
            default:
                break;
        }
        UserDataManager.Ins.SaveData();
        currentShopItem.SetStatus();
        currentShopItem.OnShopItemClick();
    }
    private void SetCoin()
    {
        currentCoin = UserDataManager.Ins.GetCoin();
        coinText.SetText($"{UserDataManager.Ins.GetCoin()}");
    }
    public void OnExitClick()
    {
        GameManager.Ins.IsNewGame = false;
        GameManager.Ins.ChangeState(GameState.MainMenu);
        GameManager.Ins.Player.OnRefresh();
        Close(0);
    }
    public void RefreshTab()
    {
        isRefresh = true;
        OnTabChange(currentTabID);
    }
    public void OnTabChange(int tabID)
    {
        if (currentTabID == tabID && !isRefresh)
            return;
        scrollRect.horizontalNormalizedPosition = 0;
        int shopItemCount = 0;
        itemClickIndex = 0;
        currentTabID = tabID;

        switch (tabID)
        {
            case 0:
                for (int i = 0; i < shopData.WeaponData.ItemDataList.Count; i++)
                {
                    if (currentItemList.Count < shopData.WeaponData.ItemDataList.Count)
                    {
                        ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                        currentItemList.Add(shopItem);
                    }
                    currentItemList[i].Setup(shopData.WeaponData.ItemDataList[i], OnChangeItem);
                    if (UserDataManager.Ins.GetWeapon() == shopData.WeaponData.ItemDataList[i].type)
                        itemClickIndex = i;
                }
                shopType = EShopType.Weapon;
                shopItemCount = shopData.WeaponData.ItemDataList.Count;
                break;
            case 1:
                for (int i = 0; i < shopData.PantData.ItemDataList.Count; i++)
                {
                    if (currentItemList.Count < shopData.PantData.ItemDataList.Count)
                    {
                        ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                        currentItemList.Add(shopItem);
                    }
                    if (UserDataManager.Ins.GetPant() == shopData.PantData.ItemDataList[i].type)
                        itemClickIndex = i;
                    currentItemList[i].Setup(shopData.PantData.ItemDataList[i], OnChangeItem);
                }
                shopType = EShopType.Pant;
                shopItemCount = shopData.PantData.ItemDataList.Count;
                break;
            case 2:
                for (int i = 0; i < shopData.HatData.ItemDataList.Count; i++)
                {
                    if (currentItemList.Count < shopData.HatData.ItemDataList.Count)
                    {
                        ShopItem shopItem = SimplePool.Spawn<ShopItem>(PoolType.ShopItem, contentTF);
                        currentItemList.Add(shopItem);
                    }
                    if (UserDataManager.Ins.GetHat() == shopData.HatData.ItemDataList[i].type)
                        itemClickIndex = i;
                    currentItemList[i].Setup(shopData.HatData.ItemDataList[i], OnChangeItem);
                }
                shopType = EShopType.Hat;
                shopItemCount = shopData.HatData.ItemDataList.Count;
                break;
        }
        for (int i = 0; i < currentItemList.Count; i++)
        {
            currentItemList[i].OnHideItem(i >= shopItemCount);
        }
        UpdateTabActivity(tabID);
        isRefresh = false;
        currentItemList[itemClickIndex].OnShopItemClick();
    }

}
