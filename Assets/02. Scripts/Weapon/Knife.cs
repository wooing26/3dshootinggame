using UnityEngine;

public class Knife : AWeaponBase
{
    public Transform                 RightHandTransform;
    public PlayerAnimationController PlayerAnimationController;
    private bool                     _isAttackEnd = true;

    private void Start()
    {
        PlayerAnimationController.OnFireAnimationEnd += AttackFinish;
    }

    public override void Equip()
    {
        base.Equip();
        _isAttackEnd = true;
    }

    public override void UnEquip()
    {
        base.UnEquip();
    }

    private void Update()
    {
        transform.position = RightHandTransform.position;
        transform.forward = RightHandTransform.up;
    }

    public override void Attack()
    {
        if (!_isAttackEnd)
        {
            return;
        }
        _isAttackEnd = false;

    }

    private void AttackFinish()
    {
        _isAttackEnd = true;
    }

    public override bool CanAttack() => (_isAttackEnd);
}
