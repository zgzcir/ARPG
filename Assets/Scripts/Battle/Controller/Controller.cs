using System.Collections.Generic;
using UnityEngine;

public abstract class Controller:MonoBehaviour
{    protected Transform camTrans;
    protected CameraController cameraController;

    
    public Animator Ani;
    public CharacterController CharacterController;

    private static readonly int Blend = Animator.StringToHash("Blend");

    
    protected bool isMove;


    protected TimerSvc timerSvc;
    
    private Vector2 inputDir;
    private static readonly int Action = Animator.StringToHash("Action");
    protected Dictionary<string,GameObject> fxDic=new Dictionary<string, GameObject>();
    protected bool SkillMove=false;
    protected float SkillMoveSpeed;
    public virtual void Init()
    {      
    
    }
    public Vector2 InputDir
    {
        protected get => inputDir;
        set
        {
            isMove = value != Vector2.zero;
            inputDir = value;
        }
    }
    public virtual void SetBlend(float blend)
    {
        Ani.SetFloat(Blend, blend);
    }

    public virtual void SetAction(int act)
    {
        Ani.SetInteger(Action, act);
    }

    public virtual void SetFX(string name,float duration)
    {
        
    }

    public  void SetSkillMoveState(bool move, float skillSpeed = 0f)
    {
        SkillMove = move;
        SkillMoveSpeed = skillSpeed;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}