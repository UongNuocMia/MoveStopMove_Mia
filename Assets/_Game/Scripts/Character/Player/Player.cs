using System.Collections.Generic;
using UnityEngine;
public class Player : Character
{
    private PlayerMovement playerMovement;

    protected override void OnInit()
    {
        base.OnInit();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (GameManager.IsState(GameState.GamePlay))
        {
            isMoving = playerMovement.IsRunning();
            if (isMoving)
            {
                ChangeAnim(Constants.RUN_ANIM);
                isAttack = false;
            }
            else
                OnStopMoving();
        }
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }
}
