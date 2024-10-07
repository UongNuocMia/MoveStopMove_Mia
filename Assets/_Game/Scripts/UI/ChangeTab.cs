using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTab : MonoBehaviour
{
    [SerializeField] private Sprite unActiveSprite, activeSprite;
    [SerializeField] private GameObject forcus;
    [SerializeField] private Image handleImage;

    private void Start()
    {
        OnDeActivityTab();
    }
    public void OnActivityTab()
    {
        forcus.SetActive(true);
        handleImage.sprite = activeSprite;
    }

    public void OnDeActivityTab()
    {
        forcus.SetActive(false);
        handleImage.sprite = unActiveSprite;
    }
}
