using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAttackState : AEnemyState
{
    private float _attackTimer = 0f;                  //  ㄴ 체크기
    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);
        _enemy.Agent.ResetPath();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        // 전이 : 공격 범위 만큼 멀어지면 -> Trace
        if (!_enemy.IsPlayerInAttackRange())
        {
            Debug.Log("상태전환 : Attack -> Trace");
            _attackTimer = 0f;
            _enemy.ChangeState(EEnemyState.Trace);
            return;
        }

        // 행동 : 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _enemy.AttackCoolTime)
        {
            Debug.Log("Attack");
            
            _attackTimer = 0f;
        }
    }
}
