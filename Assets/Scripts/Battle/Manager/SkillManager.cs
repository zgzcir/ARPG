using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;

    public void InitManager()
    {
        timerSvc = TimerSvc.Instance;
        resSvc = ResSvc.Instance;
        CommonTool.Log("Init SkillManager Done");
    }

    public void SkillAttack(EntityBase entity, int skillID)
    {
        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);
    }

    public void SkillAction(EntityBase caster, SkillCfg skillCfg, SkillActionCfg sac, int index)
    {
        var monsters = caster.BattleManager.GetEntityMonsters();
        int damage = skillCfg.SkillDamageLst[index];
        monsters.ForEach(target =>
        {
            if (IsInRange(caster.GetPos(), target.GetPos(), sac.Radius) &&
                IsInAngle(caster.GetTrans(), target.GetPos(), sac.Angel))
            {
                CalcDamage(caster, target, damage, skillCfg.DmgType);
            }
        });
    }

    private void CalcDamage(EntityBase caster, EntityBase target, int damage, DamageType dt)
    {
        int damageSum = damage;
        if (dt == DamageType.AD)
        {
            
            
            
        }
        else if (dt == DamageType.AP)
        {
        }
    }

    private bool IsInRange(Vector3 from, Vector3 to, float range)
    {
        float dis = Vector3.Distance(from, to);
        return dis <= range;
    }

    private bool IsInAngle(Transform trans, Vector3 to, float angle)
    {
        if (angle == 360) return true;
        Vector3 start = trans.forward;
        Vector3 dir = (to - trans.position);
        float ang = Vector3.Angle(start, dir);
        return ang <= angle / 2;
    }

    private void AttackDamage(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);
        List<int> actionIDs = skillCfg.SkillActionLst;
        int sum = 0;
        for (int i = 0; i < actionIDs.Count; i++)
        {
            SkillActionCfg sac = resSvc.GetSkillActionCfg(actionIDs[i]);
            sum += sac.DelayTime;
            if (sum > 0)
            {
                timerSvc.AddTimeTask(tid => { SkillAction(entity, skillCfg, sac, i); }, sum);
            }
            else
            {
                SkillAction(entity, skillCfg, sac, i);
            }
        }
    }

    private void AttackEffect(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);

        entity.SetAciton(skillCfg.AniAction);
        entity.SetFX(skillCfg.FX, skillCfg.Duration);
        skillCfg.SkillMoveLst.ForEach(sid => { CalcSkillMove(entity, sid); });
        entity.canControl = false;
        entity.SetDir(Vector2.zero);
        timerSvc.AddTimeTask(tid => entity.Idle(), skillCfg.Duration);
    }

    private void CalcSkillMove(EntityBase entity, int sid)
    {
        int sum = 0;

        SKillMoveCfg sKillMoveCfg = resSvc.GetSkillMoveCfg(sid);
        float speed = sKillMoveCfg.MoveDis / (sKillMoveCfg.MoveTime / 1000f);
        sum += sKillMoveCfg.DelayTime;
        if (sum > 0)
        {
            timerSvc.AddTimeTask(tid => { entity.SetSkillMove(true, speed); }, sum);
        }
        else
        {
            entity.SetSkillMove(true, speed);
        }

        sum += sKillMoveCfg.MoveTime;
        timerSvc.AddTimeTask(tid => { entity.SetSkillMove(false); }, sum);
    }
}