using System.Collections.Generic;
using UnityEngine;
public class Player : Character
{
    private PlayerMovement playerMovement;

    protected override void OnInit()
    {
        base.OnInit();
        currentWeapon = GameManager.Ins.GetWeapon((EWeaponType)UserData.Ins.GetWeapon());
        playerMovement = GetComponent<PlayerMovement>();
        SetUpWeapon();
        SetUpAccessories();
    }

    private void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {            
            isMoving = playerMovement.IsRunning();
            if (isMoving)
            {
                ChangeAnim(Constants.RUN_ANIM);
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
            attackRange += 5;
        }
        if (UserData.Ins.GetHead() > 0)
        {
            currentHeadGO = Instantiate(GameManager.Ins.GetHead((EHeadType)UserData.Ins.GetHead()), headPoint);
            attackSpeed += 0.08f;
        }
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }

    public void OnResult()
    {
        ChangeAnim(Constants.WIN_ANIM);
    }
    protected void OnStopMoving()
    {
        if (isCanAttack())
            OnPrepareAttack();
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            ChangeAnim(Constants.IDLE_ANIM);
    }
}
