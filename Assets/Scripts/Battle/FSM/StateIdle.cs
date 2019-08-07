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
         CommonTool.Log("pr idle");
         entity.SetBlend(Constans.BlendIdle);
     }
 
     public void Exit(EntityBase entity, params object[] args)
     {
         CommonTool.Log("ex idle");
     }
 }