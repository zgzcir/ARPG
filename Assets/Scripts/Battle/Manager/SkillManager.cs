using UnityEngine;
 
public class SkillManager : MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;
    public void InitManager()
    {timerSvc=TimerSvc.Instance;
        resSvc=ResSvc.Instance;
        CommonTool.Log("Init SkillManager Done");
    }

    public void AttackEffect(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);

        entity.SetAciton(skillCfg.AniAction);
        entity.SetFX(skillCfg.FX,skillCfg.Duration);
        int sum = 0;
        skillCfg.SkillMoveLst.ForEach(sid =>
        {
            SKillMoveCfg sKillMoveCfg = resSvc.GetSkillMoveCfg(sid);
            float speed = sKillMoveCfg.MoveDis / (sKillMoveCfg.MoveTime / 1000f);
            sum += sKillMoveCfg.DelayTime;
            if (sum>0)
            {
                timerSvc.AddTimeTask(tid => { entity.SetSkillMove(true, speed); }, sum);
            }
            else
            {
                entity.SetSkillMove(true, speed);
            }
            sum += sKillMoveCfg.MoveTime;
            timerSvc.AddTimeTask(tid =>
            {
                entity.SetSkillMove(false);
            }, sum);
        });
        timerSvc.AddTimeTask(tid => entity.Idle(), skillCfg.Duration);
    }
}