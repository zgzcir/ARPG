using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public abstract class EntityBase
{
    public AniState currentAniState = AniState.None;
    public StateManager StateManager;
    protected Controller Controller;
    public SkillManager SkillManager;
    public BattleManager BattleManager;
    public bool canControl = true;

    private BattleProps battleProps;


    public Queue<int> ComboQue = new Queue<int>();
    public int NextSkillID;
    public SkillCfg CurSkillCfg;

    public BattleProps BattleProps
    {
        get => battleProps;
        protected set => battleProps = value;
    }

    private int hp;

    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int HP
    {
        get => hp;
        set
        {
            CommonTool.Log(Controller.name + " hp:" + hp + "--->>" + value);
            SetHpVal(hp, value);
            hp = value;
            if (hp == 0)
            {
                Die();
            }
        }
    }

    public void Move()
    {
        StateManager.ChangeState(this, AniState.Move);
    }

    public void Idle()
    {
        StateManager.ChangeState(this, AniState.Idle);
    }

    public void Attack(int skillId)
    {
        StateManager.ChangeState(this, AniState.Attack, skillId);
    }

    public void Born()
    {
        StateManager.ChangeState(this, AniState.Born);
    }

    public void Die()
    {
        StateManager.ChangeState(this, AniState.Die);
    }

    public void Hit()
    {
        StateManager.ChangeState(this, AniState.Hit);
    }

    public virtual void SetBattleProps(BattleProps battleProps)
    {
        this.battleProps = battleProps;
        HP = battleProps.HP;
    }

    public virtual void SetBlend(float blend)
    {
        if (Controller != null)
        {
            Controller.SetBlend(blend);
        }
    }

    public virtual void SetDir(Vector2 dir)
    {
        if (Controller != null)
        {
            Controller.InputDir = dir;
        }
    }

    public virtual void SetAciton(int act)
    {
        if (Controller != null)
        {
            Controller.SetAction(act);
        }
    }

    public virtual void SetFX(string name, float duration)
    {
        if (Controller != null)
        {
            Controller.SetFX(name, duration);
        }
    }

    public virtual void SetSkillMove(bool move, float speed = 0f)
    {
        if (Controller != null)
        {
            Controller.SetSkillMoveState(move, speed);
        }
    }

    public virtual void SetAtkRotation(Vector2 dir, bool offset = false)
    {
        if (Controller != null)
        {
            if (offset)
                Controller.SetAtkRotationCamera(dir);
            else
                Controller.SetAtkRotationLocal(dir);
        }
    }

    public virtual void SkillAttack(int skillID)
    {
        SkillManager.SkillAttack(this, skillID);
    }

    public virtual Vector2 GetDirInput()
    {
        return Vector2.zero;
    }

    public virtual Vector3 GetPos()
    {
        return Controller.transform.position;
    }

    public virtual Transform GetTrans()
    {
        return Controller.transform;
    }


    public void SetDodge()
    {
        GameRoot.Instance.DynamicPanel.SetDodge(Controller.name);
    }

    public void SetCritical(int critical)
    {
        GameRoot.Instance.DynamicPanel.SetCritical(Controller.name, critical);
    }

    public void SetHurt(int hurt)
    {
        GameRoot.Instance.DynamicPanel.SetHurt(Controller.name, hurt);
    }

    public void SetHpVal(int oldVal, int newVal)
    {
        GameRoot.Instance.DynamicPanel.SetHpVal(Controller.name, oldVal, newVal);
    }

    public void SetActive(bool active = true)
    {
        if (Controller != null)
        {
            Controller.gameObject.SetActive(active);
        }
    }

    public void SetCtrl(Controller ctrl)
    {
        Controller = ctrl;
    }

    public List<AnimationClip> GetAnimationClips()
    {
        if (Controller != null)
        {
            return Controller.Ani.runtimeAnimatorController.animationClips.ToList();
        }

        return null;
    }

    public virtual Vector2 CalcTargetDir()
    {
        return Vector2.zero;
    }


    public void ExitCurSkill()
    {
        canControl = true;

        if (CurSkillCfg.IsCombo)
        {
            if (ComboQue.Count > 0)
            {
                NextSkillID = ComboQue.Dequeue();
            }
            else
            {
                NextSkillID = 0;
            }
        }

        SetAciton(Constans.ActionDefault);
    }
}