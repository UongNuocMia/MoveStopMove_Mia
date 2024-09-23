using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : GameUnit, IInteractable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject bulletVisual;
    [SerializeField] private BoxCollider boxCollider;
    private Character owner;
    private Vector3 distance;
    private float timeExist;
    private float distanceThreshold = 1f;
    private bool isSpawn;
    private float attackSpeed;

    private void OnInit()
    {
        timeExist = 2f;
        OnHideVisual(false);
        OnHideCollision(false);
    }

    public void SetOwner(Character character)
    {
        owner = character;
    }

    public void OnFire()
    {
        OnInit();
        isSpawn = true;
        rb.velocity = owner.ShootPoint.transform.forward * 5;
    }
    private void OnDespawn()
    {
        isSpawn = false;
        rb.velocity = Vector3.zero;
        OnHideVisual(true);
        OnHideCollision(true);
        SetPosition(owner.TF.position);
    }

    private void Update()
    {
        timeExist -= Time.deltaTime;
        if (isSpawn)
        {
            BulletRolling();
        }
        if (isSpawn && timeExist <= 0)
        {
            OnDespawn();
        }
    }

    private void BulletRolling()
    {
        float radius = 1f;
        float rollSpeed = rb.velocity.magnitude / radius;
        // Calculate rotation angle
        float rotationAngle = rollSpeed * Time.deltaTime * 360f;
        TF.Rotate(Vector3.up, rotationAngle);
    }

    public void Interact(Character character)
    {
        if (owner == character)
            return;
        owner.OnKillSucess(character);
        character.TakeDamage();
        OnDespawn();
    }
    protected void OnHideVisual(bool isHide) => bulletVisual.SetActive(!isHide);
    protected void OnHideCollision(bool isHide) => boxCollider.enabled = !isHide;

}
