using UnityEngine;

public class TraceEnemy : Enemy
{
    private void Start()
    {
        Initialize();
        ChangeState(EEnemyState.Trace);
    }

    protected override void Initialize()
    {
        base.Initialize();

        _enemyType = EEnemyType.Trace;
    }

    public override bool IsPlayerInTraceRange()
    {
        return true;
    }
}
