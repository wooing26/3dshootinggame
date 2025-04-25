using UnityEngine;

public class EnemyDeadState : AEnemyState
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
        _timer += Time.deltaTime;
        if (_timer >= _enemy.DeathTime)
        {
            // 행동 : 죽는다.
            _enemy.gameObject.SetActive(false);
        }
    }
}
