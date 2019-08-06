using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();


    public void InitManager()
    {
        Debug.Log("Init StateManager Done");
        fsm.Add(AniState.Idle, new StateIdle());
        fsm.Add(AniState.Move, new StateMove());
        fsm.Add(AniState.Attack,new StateAttack());
    }


    public void ChangeState(EntityBase entity, AniState targetState)
    {
        if (entity.currentAniState == targetState) return;

        if (fsm.ContainsKey(targetState))
        {
            if (entity.currentAniState != AniState.None)
            {
                fsm[entity.currentAniState].Exit(entity);
            }

            fsm[targetState].Enter(entity);
            fsm[targetState].Process(entity);
        }
    }
}