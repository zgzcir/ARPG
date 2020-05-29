using UnityEngine;

public class StateIdle : IState
 {
     public void Enter(EntityBase entity, params object[] args)
     {
         CommonTool.Log("en idle"+entity.EntityType);
         entity.CurrentAniState = AniState.Idle;
         entity.SetDir(Vector2.zero);
         entity.SkillEndCb = -1;
     }
     public void Process(EntityBase entity, params object[] args)
     {

      
         if (entity.NextSkillID != 0)
         {
             entity.Attack(entity.NextSkillID);
         }

         else if (entity.EntityType == EntityType.Player)
         {
             entity.CanRlsSkill = true;
         }
         
         if (entity.GetDirInput() != Vector2.zero)
         {
             entity.Move();
              entity.SetDir(entity.GetDirInput());
         }         
         entity.SetBlend(Constans.BlendIdle);
         CommonTool.Log("pr idle");
     }
 
     public void Exit(EntityBase entity, params object[] args)
     {
         CommonTool.Log("ex idle");
     }
 }