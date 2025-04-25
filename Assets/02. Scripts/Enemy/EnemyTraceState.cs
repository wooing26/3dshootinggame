using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine;
using System.Collections.Generic;

public class EnemyTraceState : AEnemyState
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
        // 전이 : 공격 범위 만큼 멀어지면 -> Return
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) > _enemy.FindDistance)
        {
            Debug.Log("상태전환 : Trace -> Return");
            _enemy.ChangeState(EEnemyState.Return);
            return;
        }

        // 전이 : 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(_enemy.transform.position, _enemy.Player.transform.position) <= _enemy.AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            _enemy.ChangeState(EEnemyState.Attack);
            return;
        }

        // 행동 : 플레이어를 추적한다.
        //Vector3 dir = (_player.transform.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _enemy.Agent.SetDestination(_enemy.Player.transform.position);
    }
}
