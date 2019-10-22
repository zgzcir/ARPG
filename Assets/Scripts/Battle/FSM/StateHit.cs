using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateHit : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState =AniState.Hit;

        entity.SetDir(Vector2.zero);
        entity.SetSkillMove(false);
        for (int i = 0; i < entity.SKillMoveCbList.Count; i++)
        {
            int tid = entity.SKillMoveCbList[i];
            TimerSvc.Instance.DelTask(tid);
            entity.SKillMoveCbList.Clear();
        }
        for (int i = 0; i < entity.SKillActionCbList.Count; i++)
        {
            int tid = entity.SKillActionCbList[i];
            TimerSvc.Instance.DelTask(tid);
            entity.SKillActionCbList.Clear();
        }
        if (entity.SkillEndCb!=-1)
        {
            TimerSvc.Instance.DelTask(entity.SkillEndCb);
            entity.SkillEndCb = -1;
        } 
        if (entity.NextSkillID!=0||entity.ComboQue.Count>0)
        {
            entity.NextSkillID = 0;
            entity.ComboQue.Clear();

            entity.BattleManager.LastAtkTime = 0;
            entity.BattleManager.ComboIndex = 0;
        }

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
        if (entity.EntityType == EntityType.Player)
        {
            AudioSource audioSource = entity.GetAudioSource();
            AudioSvc.Instance.PlayPlayerAudio(audioSource, Constans.PlayerHurtAss);
        }
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
        var r = clipsF.Count != 1 ? 1 : clipsF[0].length;
        return r;
    }
}