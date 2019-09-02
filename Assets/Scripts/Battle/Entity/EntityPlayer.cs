using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : EntityBase
{
    public EntityPlayer()
    {
        EntityType = EntityType.Player;
    }

    public override Vector2 GetDirInput()
    {
        return BattleManager.GetDirInput();
    }

    public override Vector2 CalcTargetDir()
    {
        EntityMonster monster = FindClosedTarget();
        if (monster != null)
        {
            Vector3 target = monster.GetPos();
            Vector3 self = GetPos();
            Vector2 dir = new Vector2(target.x - self.x, target.z - self.z);
            return dir.normalized;
        }

        return Vector2.zero;
    }

    private EntityMonster FindClosedTarget()
    {
        List<EntityMonster> lst = BattleManager.GetEntityMonsters();
        if (lst == null || lst.Count == 0)
        {
            return null;
        }

        Vector3 self = GetPos();
        EntityMonster targetMonster = null;
        float dis = 0;

        for (int i = 0; i < lst.Count; i++)
        {
            if (i == 0)
            {
                dis = Vector3.Distance(self, lst[i].GetPos());
                targetMonster = lst[i];
            }
            else
            {
                float calcDis = Vector3.Distance(self, lst[i].GetPos());
                if (dis > calcDis)
                {
                    dis = calcDis;
                    targetMonster = lst[i];
                }
            }
        }

        return targetMonster;
    }

    public override void SetHpVal(int oldVal, int newVal)
    {
        BattleSys.Instance.battleControllPanel.SetHpBarVal(newVal);
    }

    public override void SetDodge()
    {
        GameRoot.Instance.DynamicPanel.SetSelfDodge();
    }
}