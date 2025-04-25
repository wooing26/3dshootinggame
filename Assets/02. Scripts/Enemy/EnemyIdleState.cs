using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyIdleState : AEnemyState
{
    private float _timer;

    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);
        _timer = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        // 행동 : 가만히 있는다.
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) <= _enemy.FindDistance)
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
