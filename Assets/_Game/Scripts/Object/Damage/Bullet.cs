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
    private bool isSpawn;

    private void OnInit()
    {
        timeExist = 4f;
        OnHideVisual(false);
        OnHideCollision(false);
        OnRolling(true);
        speed = 5f;
    }

    private void OnRolling(bool v)
    {
        
    }

    public void SetOwner(Character character)
    {
        owner = character;
    }

    public void OnFire()
    {
        OnInit();
        isSpawn = true;
        rb.velocity = owner.ShootPoint.transform.forward * speed;
        Invoke(nameof(OnDeSpawn), timeExist);
    }
    private void OnDeSpawn()
    {
        isSpawn = false;
        rb.velocity = Vector3.zero;
        OnHideVisual(true);
        OnHideCollision(true);
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
