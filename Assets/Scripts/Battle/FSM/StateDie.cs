 
   
public class StateDie:IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.currentAniState = AniState.Die;
            CommonTool.Log("en Die");

            
        }

        public void Process(EntityBase entity, params object[] args)
        {
            
                entity.SetAciton(Constans.ActionDie);
                TimerSvc.Instance.AddTimeTask(tid => { entity.Controller.gameObject.SetActive(false); },
                    Constans.DieAniLength);
            CommonTool.Log("pr Die");

        }

        public void Exit(EntityBase entity, params object[] args)
        {
            CommonTool.Log("ex Die");

        }
    }
