using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingScaleUI : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private RectTransform upSizeRT;

    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = upSizeRT.localPosition;
        player.OnScaleUp += OnUpSize;
        upSizeRT.gameObject.SetActive(false);
    }
    public void OnUpSize()
    {
        upSizeRT.gameObject.SetActive(true);

        upSizeRT.DOLocalMoveY(0.5f, 0.7f).OnComplete(OnComplete);
    }

    public void OnComplete()
    {
        upSizeRT.gameObject.SetActive(false);
        upSizeRT.localPosition = defaultPosition;
    }
}
