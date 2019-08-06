public class StateAttack : IState
 {
     public void Enter(EntityBase entity)
     {
         entity.currentAniState = AniState.Attack;
         CommonTool.Log("en atk");

     }
 
     public void Process(EntityBase entity)
     {
         CommonTool.Log("pr atk");

     }
 
     public void Exit(EntityBase entity)
     {
         CommonTool.Log("ex atk");

     }
 }