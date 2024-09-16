public class IdleState : IState<Bot>
{
    public void OnEnter(Bot enemy)
    {
        enemy.ChangeAnim(Constants.IDLE_ANIM);
    }

    public void OnExecute(Bot enemy)
    {

    }

    public void OnExit(Bot enemy)
    {

    }
}
