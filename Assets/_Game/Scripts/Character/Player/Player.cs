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
        SetUpAccessories();
        playerMovement = GetComponent<PlayerMovement>();
        SetUpWeapon();
        attackArea.SetScale(AttackRange);
        CharacterName = "You";
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
        currentWeaponPrefab = GameManager.Ins.GetWeapon(UserDataManager.Ins.GetWeapon());

        SetUpPant(UserDataManager.Ins.GetPant());
        //AttackRange += 2; cho v�o h�m buff
        SetUpHat(UserDataManager.Ins.GetHat());
        //AttackSpeed += 0.08f; cho v�o h�m buff
    }
    public void OnShopping(EShopType shopType, Enum type)
    {
        switch (shopType)
        {
            case EShopType.Weapon:
                currentWeaponPrefab = GameManager.Ins.GetWeapon((EWeaponType)type);
                SetUpWeapon();
                break;
            case EShopType.Pant:
                SetUpPant((EPantType)type);
                break;
            case EShopType.Hat:
                SetUpHat((EHatType)type);
                break;
        }
    }
    public void OnRefresh()
    {
        OnInit();
        ApplyBuff();
    }
    protected void SetUpPant(EPantType pantType)
    {
        if (pantType == EPantType.None)
            pantRenderer.gameObject.SetActive(false);
        else
            pantRenderer.gameObject.SetActive(true);
        pantRenderer.material = GameManager.Ins.GetPantMaterials(pantType);
    }

    public override void OnEndGame()
    {
        base.OnEndGame();
    }
    protected void SetUpHat(EHatType hatType)
    {
        if (currentHat != null)
            Destroy(currentHat.gameObject);
        if (hatType == EHatType.None)
            return;
        currentHatPrefab = GameManager.Ins.GetHat(hatType);
        Hat hat = Instantiate(currentHatPrefab, hatPoint);
        currentHat = hat;
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }
    protected void OnStopMoving()
    {
        if (IsCanAttack())
            OnPrepareAttack();
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !IsDead)
            ChangeAnim(Constants.ISIDLE_ANIM);
    }
    protected override void OnDeath(Character character)
    {
        base.OnDeath(character);
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
