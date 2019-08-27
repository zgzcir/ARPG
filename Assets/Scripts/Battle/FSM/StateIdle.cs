using UnityEngine;

public class StateIdle : IState
 {
     public void Enter(EntityBase entity, params object[] args)
     {
         CommonTool.Log("en idle");
         entity.currentAniState = AniState.Idle;
         entity.SetDir(Vector2.zero);
     }
     public void Process(EntityBase entity, params object[] args)
     {
         if (entity.NextSkillID != 0)
         {
             entity.Attack(entity.NextSkillID);
         }
         else
         {
         if (entity.GetDirInput() != Vector2.zero)
         {
             entity.Move();
              entity.SetDir(entity.GetDirInput());
         }         
         
         entity.SetBlend(Constans.BlendIdle);
         }
         CommonTool.Log("pr idle");
     }
 
     public void Exit(EntityBase entity, params object[] args)
     {
         CommonTool.Log("ex idle");
     }
 }