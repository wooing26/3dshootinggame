using UnityEngine;

public class Knife : AWeaponBase
{
    public Transform RightHandTransform;
    public override void Equip()
    {
        base.Equip();
    }

    public override void UnEquip()
    {
        base.UnEquip();
    }

    private void Update()
    {
        transform.position = RightHandTransform.position;
    }

    public override void Attack()
    {

    }

    public override bool CanAttack() => (true);
}
