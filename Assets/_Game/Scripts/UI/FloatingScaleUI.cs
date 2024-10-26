using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingScaleUI : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private RectTransform upSizeRT;


    private void Start()
    {
        player.OnScaleUp += OnUpSize;
        upSizeRT.gameObject.SetActive(false);
    }

    public void OnUpSize()
    {
        upSizeRT.gameObject.SetActive(true);

        Vector3 newPosition = upSizeRT.transform.position + new Vector3(Random.Range(-2, 2), 1.5f);
        upSizeRT.DOMove(newPosition, 0.5f).OnComplete(OnComplete);
    }

    public void OnComplete()
    {
        upSizeRT.gameObject.SetActive(false);
        upSizeRT.anchoredPosition = Vector2.zero;
    }
}
