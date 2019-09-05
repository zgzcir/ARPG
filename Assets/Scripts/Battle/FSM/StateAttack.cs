public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        entity.CurSkillCfg = ResSvc.Instance.GetSkillCfg((int) args[0]);
       
        CommonTool.Log("en atk");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.EntityType==EntityType.Player)
        {
            entity.CanRlsSkill = false;
        }
        
        CommonTool.Log("pr atk");
   
        entity.SkillAttack((int) args[0]);
    }
    public void Exit(EntityBase entity, params object[] args)
    {
        entity.ExitCurSkill();
        CommonTool.Log("ex atk");
    }
}