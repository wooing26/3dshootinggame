using UnityEngine;

public class TraceEnemy : Enemy
{
    private void Start()
    {
        Initialize();
        ChangeState(EEnemyState.Trace);
    }

    public override bool IsPlayerInTraceRange()
    {
        return true;
    }
}
