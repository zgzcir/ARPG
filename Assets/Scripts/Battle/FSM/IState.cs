public interface IState
{
    void Enter(EntityBase entity);

    void Process(EntityBase entity);

    void Exit(EntityBase entity);
    
    
}

public enum AniState
{
None,
Idle,
Move,
Attack
    
    
}