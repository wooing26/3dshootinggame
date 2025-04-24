using UnityEngine;

public class EnemyDamagedState : AEnemyState
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
        _characterController.Move(_knockBackDir * _knockBackPower * Time.deltaTime);
    }
}
