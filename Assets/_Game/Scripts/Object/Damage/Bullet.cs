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
    private float attackSpeed;
    private float attackRange;
    private float distanceThreshold = 0.1f;
    private bool isSpawn;

    private void OnInit()
    {
        timeExist = 4f;
        attackSpeed = 3f; // deafult
        attackRange = 10f;// deafult
        OnHideVisual(false);
        OnHideCollision(false);
    }

    public void SetOwner(Character character)
    {
        owner = character;
    }

    public void SetStats(float attackSpeed,float attackRange)
    {
        this.attackSpeed += attackSpeed;
        this.attackRange += attackRange;
    }

    public void OnFire()
    {
        OnInit();
        isSpawn = true;
        rb.velocity = owner.ShootPoint.transform.forward * attackSpeed;
        Vector3 distance = owner.ShootPoint.transform.forward * attackRange;
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
        if (isSpawn)
        {
            float radius = 1f;
            float rollSpeed = rb.velocity.magnitude / radius;
            // Calculate rotation angle
            float rotationAngle = rollSpeed * Time.deltaTime * 360f;
            TF.Rotate(Vector3.up, rotationAngle);
        }
        if (Vector3.Distance(distance, TF.position) <= distanceThreshold)
        {
            OnDespawn();
        }
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
