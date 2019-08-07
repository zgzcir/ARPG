public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        CommonTool.Log("en atk");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        CommonTool.Log("pr atk");
        entity.AttackEffect((int)args[0]);
    }
    public void Exit(EntityBase entity, params object[] args)
    {
        CommonTool.Log("ex atk");
    }
}