using UnityEngine;

public abstract  class EntityBase
{
    public AniState currentAniState = AniState.None;
    public StateManager StateManager;
    public Controller Controller;
    
    public void Move()
    {
        StateManager.ChangeState(this,AniState.Move);
    }
    public void Idle()
    {
        StateManager.ChangeState(this,AniState.Idle);
    }

    public void Attack()
    {
        StateManager.ChangeState(this,AniState.Attack);
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
}