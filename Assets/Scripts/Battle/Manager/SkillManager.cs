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
        entity.SKillActionCbList.Clear();
        entity.SKillMoveCbList.Clear();
        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);
    }

    public void SkillAction(EntityBase caster, SkillCfg skillCfg, SkillActionCfg sac, int index)
    {
        var monsters = caster.BattleManager.GetEntityMonsters();
        int damage = skillCfg.SkillDamageLst[index];

        if (caster.EntityType == EntityType.Monster)
        {
            EntityPlayer targetPlayer = caster.BattleManager.EntitySelfplayer;
            if (IsInRange(caster.GetPos(), targetPlayer.GetPos(), sac.Radius) &&
                IsInAngle(caster.GetTrans(), targetPlayer.GetPos(), sac.Angel))
            {
                CalcDamage(caster, targetPlayer, damage, skillCfg.DmgType);
            }
        }
        else if (caster.EntityType == EntityType.Player)
        {
            monsters.ForEach(targetMonster =>
            {
                if (IsInRange(caster.GetPos(), targetMonster.GetPos(), sac.Radius) &&
                    IsInAngle(caster.GetTrans(), targetMonster.GetPos(), sac.Angel))
                {
                    CalcDamage(caster, targetMonster, damage, skillCfg.DmgType);
                }
            });
        }
    }

    System.Random rd = new System.Random();

    private void CalcDamage(EntityBase caster, EntityBase target, int damage, DamageType dt)
    {
        int damageSum = damage;
        if (dt == DamageType.AD)
        {
            {
                //闪避
                int dodgeNum = ZCTools.RDInt(1, 100, rd);
                if (dodgeNum <= target.BattleProps.Dodge)
                {
                    //     GameRoot.AddTips(target.Controller.name + "闪避了你的攻击"); //todo转移到聊天窗口
                    //   CommonTool.Log("闪避Rate:" + dodgeNum + "/" + target.BattleProps.Dodge);
                    if (target.EntityType == EntityType.Player)
                    {
                        target.SetDodge();
                    }

                    return;
                }
            }
            damageSum += caster.BattleProps.PA;
            {
                //暴击
                int criticalNum = ZCTools.RDInt(1, 100, rd);
                if (criticalNum <= caster.BattleProps.Critical)
                {
                    float criticalIncRate = (1 + ZCTools.RDInt(1, 100, rd) / 100.0f);
                    //       CommonTool.Log("暴击ratr"+criticalIncRate);
                    damageSum = (int) (criticalIncRate * damageSum);
                    target.SetCritical(damageSum);
                }
            }
            {
                //穿甲
                int pd = (int) ((1 - caster.BattleProps.Pierce / 100.0f) * target.BattleProps.PD);
                damageSum -= pd;
                CommonTool.Log("对" + target.Name + "造成了" + damageSum + "点物理伤害");
            }
        }
        else if (dt == DamageType.AP)
        {
            damageSum += caster.BattleProps.SA;
            damageSum -= target.BattleProps.SD;
        }

        if (damageSum <= 0)
        {
            return;
        }

        if (target.HP <= damageSum)
        {
            target.HP = 0;
            target.BattleManager.RemoveMonster(target.Name);
        }
        else
        {
            target.HP -= damageSum;
            if (target.EntityState==EntityState.None&&target.GetBreak())
            {
                target.Hit();
            }
        }
        target.SetHurt(damageSum);
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
                var i1 = i;
                int actID =
                    timerSvc.AddTimeTask(tid =>
                    {
                        SkillAction(entity, skillCfg, sac, i1);
                        entity.RemoveActionCB(tid);
                    }, sum);
                entity.SKillActionCbList.Add(actID);
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
        if (!skillCfg.IsCollide)
        {
            Physics.IgnoreLayerCollision(9, 10);
            timerSvc.AddTimeTask(tid => { Physics.IgnoreLayerCollision(9, 10, false); }, skillCfg.Duration);
        }

        if (entity.EntityType == EntityType.Player)
        {
            if (entity.GetDirInput() == Vector2.zero)
            {
                Vector2 dir = entity.CalcTargetDir();
                if (dir != Vector2.zero)
                {
                    entity.SetAtkRotation(dir);
                }
            }
            else
            {
                entity.SetAtkRotation(entity.GetDirInput(), true);
            }
        }

        entity.SetAciton(skillCfg.AniAction);
        entity.SetFX(skillCfg.FX, skillCfg.Duration);
        skillCfg.SkillMoveLst.ForEach(sid => { CalcSkillMove(entity, sid); });
        entity.CanControl = false;
        entity.SetDir(Vector2.zero);
        if (!skillCfg.IsBreak)
        {
            entity.EntityState = EntityState.ButyState;
        }

        timerSvc.AddTimeTask(tid => entity.Idle(), skillCfg.Duration); //<<
    }

    private void CalcSkillMove(EntityBase entity, int sid)
    {
        int sum = 0;

        SKillMoveCfg sKillMoveCfg = resSvc.GetSkillMoveCfg(sid);
        float speed = sKillMoveCfg.MoveDis / (sKillMoveCfg.MoveTime / 1000f);
        sum += sKillMoveCfg.DelayTime;
        if (sum > 0)
        {
            int moveID = timerSvc.AddTimeTask(tid =>
            {
                entity.SetSkillMove(true, speed);
                entity.RemoveMoveCB(tid);
            }, sum);
            entity.SKillMoveCbList.Add(moveID);
        }
        else
        {
            entity.SetSkillMove(true, speed);
        }

        sum += sKillMoveCfg.MoveTime;
        int stopID =
            timerSvc.AddTimeTask(tid =>
            {
                entity.SetSkillMove(false);
                entity.RemoveMoveCB(tid);
            }, sum);
        entity.SKillMoveCbList.Add(stopID);
    }
}