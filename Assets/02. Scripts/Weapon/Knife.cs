using UnityEngine;

public class Knife : AWeaponBase
{
    public Transform                 RightHandTransform;
    public PlayerAnimationController PlayerAnimationController;
    public float                     AttackAngle  = 30f;
    public float                     AttackRadius = 15f;
    public int                       KnifeDamage  = 20;

    private bool                     _isAttackEnd = true;
    private float                    _attackCosValue;

    private void Start()
    {
        PlayerAnimationController.OnFireAnimationEnd += AttackFinish;
        _attackCosValue = Mathf.Cos(AttackAngle * Mathf.Deg2Rad);
    }

    public override void Equip(Player player)
    {
        base.Equip(player);
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

        Collider[] overlapColliders = Physics.OverlapSphere(_player.transform.position, AttackRadius);
        
        foreach(Collider collider in overlapColliders)
        {
            if (collider.gameObject == gameObject)
            {
                continue;
            }

            Vector3 targetVector = (collider.transform.position - transform.position).normalized;
            float attackAngleDot = Vector3.Dot(_player.transform.forward, targetVector);
            if (attackAngleDot < _attackCosValue)
            {
                continue;
            }

            IDamageable damagedEntity = collider.GetComponent<IDamageable>();
            if (damagedEntity == null)
            {
                continue;
            }

            Damage damage = new Damage();
            damage.Value = KnifeDamage;
            damage.KnockBackPower = 10f;
            damage.From = this.gameObject;

            damagedEntity.TakeDamage(damage);
        }
    }

    private void AttackFinish()
    {
        _isAttackEnd = true;
    }

    public override bool CanAttack() => (_isAttackEnd);
}
