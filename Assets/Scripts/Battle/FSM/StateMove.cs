public class StateMove : IState
 {
     public void Enter(EntityBase entity, params object[] args)
     {
         CommonTool.Log("en move");
         
         entity.currentAniState = AniState.Move;

//         if (entity.NextSkillID != 0 || entity.ComboQue.Count > 0)
//         {
//             entity.NextSkillID = 0;
//             entity.ComboQue.Clear();
//
//             entity.BattleManager.LastAtkTime = 0;
//             entity.BattleManager.ComboIndex = 0;
//         }
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