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
           SKillMoveCfg sKillMoveCfg = resSvc.GetSkillMoveCfg(skillCfg.SkillMove);
           float speed = sKillMoveCfg.MoveDis / (sKillMoveCfg.MoveTime / 1000f);
           entity.SetSkillMove(true,speed);
           timerSvc.AddTimeTask(tid =>
           {
         entity.SetSkillMove(false);
           }, sKillMoveCfg.MoveTime);
           timerSvc.AddTimeTask(tid => entity.Idle(), skillCfg.Duration);
     }
 }