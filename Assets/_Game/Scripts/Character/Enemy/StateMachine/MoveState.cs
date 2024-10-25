using UnityEngine;

public class MoveState : IState<Bot>
{
    private Vector3 randomPosition;
    public void OnEnter(Bot bot)
    {
        randomPosition = bot.RandomPosition();
        bot.Move(randomPosition);
    }

    public void OnExecute(Bot bot)
    {
        if(Vector3.Distance(bot.TF.position, randomPosition) < 0.1f)
        {
            bot.SetMoving(false);
            bot.ChangeState(new IdleState());
        }
    }
    
    public void OnExit(Bot bot)
    {

    }

}
