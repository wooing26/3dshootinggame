using UnityEngine;

public class TraceEnemy : Enemy
{

    public override void Initialize(Vector3 spawnPoint)
    {
        base.Initialize(spawnPoint);

        //ChangeState(EEnemyState.Trace);
    }

    public override bool IsPlayerInTraceRange()
    {
        return true;
    }
}
