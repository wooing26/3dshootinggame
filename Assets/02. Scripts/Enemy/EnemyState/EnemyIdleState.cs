using UnityEngine;

public class EnemyIdleState : AEnemyState
{
    private float _timer;

    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);
        _timer = 0f;
        Debug.Log($"Idle : 이전 위치 : {_enemy.transform.position}");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log($"Idle : 이후 위치 : {_enemy.transform.position}");
    }

    public override void Update()
    {
        // 행동 : 가만히 있는다.
        if (_enemy.IsPlayerInTraceRange())
        {
            Debug.Log("상태전환 : Idle -> Trace");
            _timer = 0f;
            _enemy.ChangeState(EEnemyState.Trace);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _enemy.IdleWaitTime)
        {
            Debug.Log("상태전환 : Idle -> Patrol");
            _timer = 0f;
            _enemy.ChangeState(EEnemyState.Patrol);
            return;
        }
    }
}
