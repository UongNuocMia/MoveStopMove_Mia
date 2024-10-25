using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform uiHandleRectTransform;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Sprite backgroundActiveSprite;
    [SerializeField] private Sprite backgroundUnactiveSprite;

    private Toggle toggle;

    private Vector2 handlePosition;

    private void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = uiHandleRectTransform.anchoredPosition;
        toggle.onValueChanged.AddListener(OnSwitch);
        OnSwitch(toggle.isOn);
    }

    private void OnSwitch( bool isOn)
    {
        Vector2 newPosition = isOn ? new Vector2(handlePosition.x * -1, handlePosition.y) :
                                                   handlePosition;
        //uiHandleRectTransform.DOAnchorPos(newPosition, 0.5f);
        uiHandleRectTransform.anchoredPosition = newPosition;
        backGroundImage.sprite = isOn ? backgroundActiveSprite : backgroundUnactiveSprite;
    }
}
