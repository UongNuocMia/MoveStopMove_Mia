using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject lockGO, equippedGO;

    private void OnInit()
    {
        lockGO.SetActive(true);
        if (UserData.Ins.IsHaveThisItem)
        {
            lockGO.SetActive(false);
        }
       //if() UserData.Ins.GetWeapon()
    }
}
