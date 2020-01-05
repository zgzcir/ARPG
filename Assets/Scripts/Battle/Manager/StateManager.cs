using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();


    public void InitManager()
    {
        CommonTool.Log("Init StateManager Done");
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Move, new StateMove());
        fsm.Add(AniState.Attack,new StateAttack());
        fsm.Add(AniState.Born,new StateBorn());
        fsm.Add(AniState.Die,new StateDie());
        fsm.Add(AniState.Hit,new StateHit());
    }
    
    public void ChangeState(EntityBase entity, AniState targetState,params object[]args)
    {
        if (entity.CurrentAniState == targetState) return;

        if (fsm.ContainsKey(targetState))
        {
            if (entity.CurrentAniState != AniState.None)
            {
                fsm[entity.CurrentAniState].Exit(entity,args);
            }
            fsm[targetState].Enter(entity,args);
            fsm[targetState].Process(entity,args);
        }
    }
}