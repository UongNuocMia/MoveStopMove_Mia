using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.Move(bot.TF.position);
        bot.SetMoving(false);
        bot.ChangeAnim(Constants.ISDEAD_ANIM);
        bot.OnHideTargetSprite(true);
        bot.OnHideCollider(true);
        LevelManager.Ins.OnReviveBot(bot);
    }

    public void OnExecute(Bot bot)
    {
       
    }

    public void OnExit(Bot bot)
    {

    }
}
