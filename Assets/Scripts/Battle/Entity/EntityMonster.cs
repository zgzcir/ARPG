using UnityEngine;

public class EntityMonster : EntityBase
{
    public EntityMonster()
    {
        EntityType = EntityType.Monster;
    }

    public MonsterMapData MonsterMapData;

    public override void SetBattleProps(BattleProps battleProps)
    {
        var level = MonsterMapData.MLevel;

        BattleProps = new BattleProps()
        {
            HP = battleProps.HP * level,
            SA = battleProps.SA * level,
            PA = battleProps.PA * level,
            SD = battleProps.SD * level,
            PD = battleProps.PD * level,
            Dodge = battleProps.Dodge * level,
            Pierce = battleProps.Pierce * level,
            Critical = battleProps.Critical * level,
        };
        HP = BattleProps.HP;
    }

    private float checkTime = 2f;
    private float checkCount = 0;
    private bool runAi = true;

    private float atkTime = 2;
    private float atkCount;

    public override void TickAILogic()
    {
        if (!runAi)
        {
            Idle();
            return;
        }
        if (CurrentAniState == AniState.Idle || CurrentAniState == AniState.Move)
        {
            float deltaTime = Time.deltaTime;
            checkCount += deltaTime;
            atkCount += deltaTime;
            if ( checkCount< checkTime)
                return;
            Vector2 dir = CalcTargetDir();
            if (!IsInAtkRange())
            {
                SetDir(dir);
                Move();
            }
            else
            {
                SetDir(Vector2.zero);
                if (atkCount >=atkTime )
                {
                    SetAtkRotation(dir);
                    Attack(MonsterMapData.MCfg.SkillID);
                    atkCount = 0;
                }
                else
                {
                    Idle();
                }
            }

            checkCount = 0;
            checkTime = ZCTools.RDInt(1, 5) * 1.0f / 10;
        }
    }

    public override bool IsInAtkRange()
    {
        EntityPlayer entityPlayer = BattleManager.EntitySelfplayer;
        if (entityPlayer == null || entityPlayer.CurrentAniState == AniState.Die)
        {
            runAi = false;
            return false;
        }

        Vector3 target = entityPlayer.GetPos();
        Vector3 self = GetPos();
        target.y = 0;
        self.y = 0;
        float dis = Vector3.Distance(self, target);
        if (dis <= MonsterMapData.MCfg.AtkDis)
        {
            return true;
        }

        return false;
    }

    public override Vector2 CalcTargetDir()
    {
        EntityPlayer entityPlayer = BattleManager.EntitySelfplayer;
        if (entityPlayer == null || entityPlayer.CurrentAniState == AniState.Die)
        {
            runAi = false;
            return Vector2.zero;
        }

        Vector3 target = entityPlayer.GetPos();
        Vector3 self = GetPos();
        return new Vector2(target.x - self.x, target.z - self.z).normalized;
    }

    public override bool GetBreak()
    {
        if (MonsterMapData.MCfg.IsStop)
        {
            if (CurSkillCfg != null)
            {
                return CurSkillCfg.IsBreak;
            }
                return MonsterMapData.MCfg.IsStop;
        }
        return false;
    }
}