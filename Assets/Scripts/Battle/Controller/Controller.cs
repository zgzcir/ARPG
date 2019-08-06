using UnityEngine;

public abstract class Controller:MonoBehaviour
{
    public Animator Ani;
    private static readonly int Blend = Animator.StringToHash("Blend");

    
    protected bool isMove;

    
    private Vector2 inputDir;
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

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}