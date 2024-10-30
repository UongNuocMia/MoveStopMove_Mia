
using UnityEngine;

public class AttackState : IState<Bot>
{
    private float timeToAttack;
    private float timeToDoNext;
    public void OnEnter(Bot bot)
    {
        timeToAttack = 0.5f;
        timeToDoNext = 2f;
    }

    public void OnExecute(Bot bot)
    {
        timeToAttack -= Time.deltaTime;
        if (timeToAttack <= 0)
        {
            if (bot.IsCanAttack())
                bot.OnPrepareAttack();
            timeToDoNext-= Time.deltaTime;
            if (timeToDoNext <= 0)
            {
                bot.ChangeState(new MoveState());
            }
        }
    }

    public void OnExit(Bot enemy)
    {

    }
}
