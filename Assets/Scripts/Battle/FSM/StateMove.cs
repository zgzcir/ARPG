public class StateMove : IState
 {
     public void Enter(EntityBase entity)
     {
         CommonTool.Log("en move");
         entity.currentAniState = AniState.Move;
     }
 
     public void Process(EntityBase entity)
     {
         CommonTool.Log("pr move");
         entity.SetBlend(Constans.BlendMove);
     }
 
     public void Exit(EntityBase entity)

     {
         CommonTool.Log("ex move");
         

     }
 }