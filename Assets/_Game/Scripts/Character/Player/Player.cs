using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Character
{
    public event EventHandler<OnKillEnemyEventArgs> OnKillEnemy;
    public class OnKillEnemyEventArgs : EventArgs
    {
        public float score;
    }

    private PlayerMovement playerMovement;
    protected override void OnInit()
    {
        base.OnInit();
        currentWeaponPrefab = GameManager.Ins.GetWeapon((EWeaponType)UserDataManager.Ins.GetWeapon());
        playerMovement = GetComponent<PlayerMovement>();
        SetUpWeapon();
        SetUpAccessories();
        attackArea.SetScale(AttackRange);
    }
    private void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {            
            isMoving = playerMovement.IsRunning();
            if (isMoving)
            {
                ChangeAnim(Constants.ISRUN_ANIM);
                isAttacked = false;
            }
            else
            {
                if (!IsDead)
                    OnStopMoving();         
            }
        }
    }
    protected override void SetUpAccessories()
    {
        if (UserDataManager.Ins.GetPant() > 0)
        {
            pantRenderer.material = GameManager.Ins.GetPantMaterials((EPantType)UserDataManager.Ins.GetPant());
            AttackRange += 2;
        }
        if (UserDataManager.Ins.GetHat() > 0 && currentHat != null)
        {
            currentHatPrefab = GameManager.Ins.GetHat((EHatType)UserDataManager.Ins.GetHat());
            GameObject hatGO = Instantiate(currentHatPrefab, hatPoint);
            currentHat = hatGO;
            AttackSpeed += 0.08f;
        }
    }
    public float GetPlayerSpeed()
    {
        return speed;
    }
    public void OnResult()
    {
        ChangeAnim(Constants.ISWIN_ANIM);
    }
    protected void OnStopMoving()
    {
        if (IsCanAttack())
            OnPrepareAttack();
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !IsDead)
            ChangeAnim(Constants.ISIDLE_ANIM);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        float time = Utilities.GetTimeCurrentAnim(anim, Constants.DEAD_ANIM);
        GameManager.Ins.IsPlayerWin = false;
        GameManager.Ins.ChangeState(GameState.Finish); 
    }
    public override void OnKillSuccess(Character character)
    {
        base.OnKillSuccess(character);
        OnKillEnemy?.Invoke(this, new OnKillEnemyEventArgs
        {
            score = 2
        });
    }

    public void OnShopping()
    {

    }
}
