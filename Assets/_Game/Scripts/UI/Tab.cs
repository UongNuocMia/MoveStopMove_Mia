using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private Sprite unActiveSprite, activeSprite;
    [SerializeField] private GameObject forcus;
    [SerializeField] private Image handleImage;

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
