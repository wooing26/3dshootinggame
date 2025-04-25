using UnityEngine;

public class EnemyDamagedState : AEnemyState
{
    private Vector3     _knockBackDir;
    private float       _knockBackPower;

    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);
    }

    public void SetKnockBack(Vector3 knockBackDir, float knockBackPower)
    {
        _knockBackDir = knockBackDir;
        _knockBackPower = knockBackPower;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        _enemy.CharacterController.Move(_knockBackDir * _knockBackPower * Time.deltaTime);
    }
}
