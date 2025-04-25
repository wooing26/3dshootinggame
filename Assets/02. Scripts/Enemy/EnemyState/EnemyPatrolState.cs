using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : AEnemyState
{
    private int _patrolIndex = 0;
    private List<Vector3> _patrolPositions;

    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);

        _patrolPositions = new List<Vector3>(_enemy.PatrolTransforms.Length);
        foreach (Transform transform in _enemy.PatrolTransforms)
        {
            _patrolPositions.Add(transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        // 순찰 구역 가면 다시 Idle 상태로 변환
        if (Vector3.Distance(_enemy.transform.position, _patrolPositions[_patrolIndex]) <= _enemy.ReturnDistance)
        {
            Debug.Log("상태전환 : Patrol -> Idle");
            _enemy.transform.position = _patrolPositions[_patrolIndex];
            _enemy.ECurrentState = EEnemyState.Idle;
            _patrolIndex = (_patrolIndex + 1) % _patrolPositions.Count;
            return;
        }

        // 중간에 플레이어 찾으면 Trace로 변경
        if (_enemy.IsPlayerInTraceRange())
        {
            Debug.Log("상태전환 : Patrol -> Trace");
            _enemy.ChangeState(EEnemyState.Trace);
            return;
        }

        // 순찰 구역으로 이동
        // Vector3 dir = (_patrolPositions[_patrolIndex] - transform.position).normalized;
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _enemy.Agent.SetDestination(_patrolPositions[_patrolIndex]);
    }
}
