
    public class StateBorn:IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.currentAniState = AniState.Born;
            CommonTool.Log("en Born");

        }
        public void Process(EntityBase entity, params object[] args)
        {

            entity.SetAciton(Constans.ActionBorn);
            TimerSvc.Instance.AddTimeTask(tid =>
            {
                entity.SetAciton(Constans.ActionDefault);
                
            }, 500);

            CommonTool.Log("pr Born");
        }

        public void Exit(EntityBase entity, params object[] args)
        {
            CommonTool.Log("ex Born");

        }
    }
