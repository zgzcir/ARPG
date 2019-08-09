using UnityEngine;

public abstract  class EntityBase
{
    public AniState currentAniState = AniState.None;
    public StateManager StateManager;
    public Controller Controller;
    public SkillManager SkillManager;
    public void Move()
    {
        StateManager.ChangeState(this,AniState.Move);
    }
    public void Idle()
    {
        StateManager.ChangeState(this,AniState.Idle);
    }

    public void Attack(int skillId)
    {
        StateManager.ChangeState(this,AniState.Attack,skillId);
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
        if (Controller!=null)
        {
            Controller.InputDir = dir;
        }
    }

    public virtual void SetAciton(int act)
    {
        if (Controller!=null)
        {
            Controller.SetAction(act);
        }
    }

    public virtual void SetFX(string name, float duration)
    {
        if (Controller != null)
        {
            Controller.SetFX(name,duration);
        }
    }

    public virtual void SetSkillMove(bool move,float speed=0f)
    {
        if (Controller != null)
        {
            Controller.SetSkillMoveState(move,speed);
        }
    }
    public virtual void AttackEffect(int skillID)
    {
        SkillManager.AttackEffect(this,skillID);
    }
}