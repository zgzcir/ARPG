using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        CommonTool.Log("en Hit");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.EntityType == EntityType.Player)
        {
            entity.CanRlsSkill = false;
        }    
        entity.SetDir(Vector2.zero);
        entity.SetAciton(Constans.ActionHit);
        TimerSvc.Instance.AddTimeTask(tid =>
            {
                entity.SetAciton(Constans.ActionDefault);
                entity.Idle();
            }, (int) GetHitLen(entity) * 1000);
        CommonTool.Log("pr Hit");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        CommonTool.Log("ex Hit");
    }

    private float GetHitLen(EntityBase entity)
    {
        List<AnimationClip> clips = entity.GetAnimationClips();
        var clipsF = clips.Where(c =>
        {
            return c.name.Contains("hit") || c.name.Contains("Hit") || c.name.Contains("HIT");
        }).ToList();
        var r=clipsF.Count != 1 ? 1 : clipsF[0].length;
        return r;
    }
}