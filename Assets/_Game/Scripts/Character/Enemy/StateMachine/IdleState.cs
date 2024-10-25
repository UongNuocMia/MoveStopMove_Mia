using UnityEngine;

public class IdleState : IState<Bot>
{
    private float timeToMove = 2f;

    public void OnEnter(Bot bot)
    {
        bot.ChangeAnim(Constants.ISIDLE_ANIM);
    }

    public void OnExecute(Bot bot)
    {
        if (!GameManager.IsState(GameState.GamePlay)) return;
        if (bot.CharacterInAreaList.Count > 0)
        {
            bot.ChangeState(new AttackState());
        }
        timeToMove -= Time.deltaTime;
        if (timeToMove <= 0)
            bot.ChangeState(new MoveState());
    }

    public void OnExit(Bot bot)
    {

    }
}
