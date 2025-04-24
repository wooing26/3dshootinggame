using UnityEngine;

public abstract class AEnemyState
{
    protected Enemy _enemy;
    public virtual void Enter(Enemy enemy)
    {
        _enemy = enemy;
    }

    public abstract void Update();
    public virtual void Exit()
    {

    }
}
