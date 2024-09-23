using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : GameUnit, IInteractable
{
    [SerializeField] protected GameObject boosterVisual;
    [SerializeField] protected BoxCollider boxCollider;
    protected EBoosterType booster;
    

    private void Start()
    {
        OnInit();
    }

    protected void OnHideVisual(bool isHide) => boosterVisual.SetActive(!isHide);
    protected void OnHideCollision(bool isHide) => boxCollider.enabled = !isHide;

    protected virtual void OnInit()
    {

    }
    public void Interact(Character character)
    {
        character.GetBooster(booster);
        OnHideVisual(true);
        OnHideCollision(true);
    }
}
