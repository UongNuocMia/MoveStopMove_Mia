using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit, IInteractable
{
    [SerializeField] private GameObject visualBullet;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Rigidbody rb;
    private Character owner;
    private float timeExist;
    private float speed;

    private void OnInit()
    {
        timeExist = 4f;
        OnHideVisual(false);
        OnHideCollision(false);
        speed = 5f;
    }

    public void SetOwner(Character character)
    {
        owner = character;
    }

    public void OnFire()
    {
        OnInit();
        rb.velocity = owner.ShootPoint.transform.forward * speed;
        Invoke(nameof(OnDeSpawn), timeExist);
    }
    private void OnDeSpawn()
    {
        rb.velocity = Vector3.zero;
        OnHideVisual(true);
        OnHideCollision(true);
    }

    public void Interact(Character character)
    {
        if (owner == character)
            return;
        owner.OnKillSucess(character);
        character.TakeDamage();
        OnDeSpawn();
    }
    protected void OnHideVisual(bool isHide) => visualBullet.SetActive(!isHide);
    protected void OnHideCollision(bool isHide) => boxCollider.enabled = !isHide;

}
