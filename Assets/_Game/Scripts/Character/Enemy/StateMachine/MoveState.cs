using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : IState<Bot>
{
    private float timeToMove;
    private Vector3 randomPosition;
    public void OnEnter(Bot bot)
    {
        randomPosition = bot.RandomPosition();
        bot.Move(randomPosition);
        timeToMove = 2f;
    }

    public void OnExecute(Bot bot)
    {
        if(bot.TF.position == randomPosition)
        {
            bot.ChangeAnim(Constants.IDLE_ANIM);
            if (bot.CharacterInAreaList.Count > 0)
            {
                bot.ChangeState(new AttackState());
            }
            timeToMove -= Time.deltaTime;
            if (timeToMove<= 0)
                bot.ChangeState(new MoveState());
        }
    }
    
    public void OnExit(Bot bot)
    {

    }

}
