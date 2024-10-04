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
        currentWeaponPrefab = GameManager.Ins.GetWeapon((EWeaponType)UserData.Ins.GetWeapon());
        playerMovement = GetComponent<PlayerMovement>();
        SetUpWeapon();
        SetUpAccessories();
        attackArea.SetScale(attackRange);
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
        if (UserData.Ins.GetPant() > 0)
        {
            pantRenderer.material = GameManager.Ins.GetPantMaterials((EPantType)UserData.Ins.GetPant());
            attackRange += 2;
        }
        if (UserData.Ins.GetHead() > 0 && currentHead != null)
        {
            currentHeadPrefab = GameManager.Ins.GetHead((EHeadType)UserData.Ins.GetHead());
            GameObject headGO = Instantiate(currentHeadPrefab, headPoint);
            currentHead = headGO;
            attackSpeed += 0.08f;
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
        if (isCanAttack())
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

}
