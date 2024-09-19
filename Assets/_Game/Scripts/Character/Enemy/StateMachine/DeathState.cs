using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState<Bot>
{
    private float onGroundTime;
    private float reviveTime;
    public void OnEnter(Bot bot)
    {
        onGroundTime = 2f;
        reviveTime = 2f;
        bot.Move(bot.TF.position);
        bot.ChangeAnim(Constants.DEAD_ANIM);
    }

    public void OnExecute(Bot bot)
    {
        onGroundTime -= Time.deltaTime;
        if(onGroundTime <= 0)
        {
            bot.OnHideVisual(true);
            reviveTime -= Time.deltaTime;
            if(reviveTime <= 0)
            {
                bot.OnRevive();
            }
        }
    }

    public void OnExit(Bot bot)
    {

    }
}
