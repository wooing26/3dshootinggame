using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyReturnState : AEnemyState
{
    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        // 전이 : 시작 위치와 가까워 지면 -> Idle
        if (Vector3.Distance(_enemy.transform.position, _enemy.StartPosition) <= _enemy.ReturnDistance)
        {
            Debug.Log("상태전환 : Return -> Idle");
            _enemy.transform.position = _enemy.StartPosition;
            _enemy.ChangeState(EEnemyState.Idle);
            return;
        }

        // 전이 : 시작 위치와 가까워 지면 -> Trace
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) <= _enemy.FindDistance)
        {
            Debug.Log("상태전환 : Return -> Trace");
            _enemy.ChangeState(EEnemyState.Trace);
            return;
        }

        // 행동 :처음 자리로 되돌아간다.
        //Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _enemy.Agent.SetDestination(_enemy.StartPosition);
    }
}
