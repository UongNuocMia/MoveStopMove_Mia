public class IdleState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.ChangeAnim(Constants.ISIDLE_ANIM);
    }

    public void OnExecute(Bot bot)
    {

    }

    public void OnExit(Bot bot)
    {

    }
}
