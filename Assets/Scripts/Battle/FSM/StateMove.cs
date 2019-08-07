public class StateMove : IState
 {
     public void Enter(EntityBase entity, params object[] args)
     {
         CommonTool.Log("en move");
         entity.currentAniState = AniState.Move;
     }
 
     public void Process(EntityBase entity, params object[] args)
     {
         CommonTool.Log("pr move");
         entity.SetBlend(Constans.BlendMove);
     }
 
     public void Exit(EntityBase entity, params object[] args)

     {
         CommonTool.Log("ex move");

     }
 }