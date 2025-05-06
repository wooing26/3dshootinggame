using UnityEngine;

public abstract class AWeaponBase : MonoBehaviour
{
    public virtual void  Equip()
    {
        gameObject.SetActive(true);
    }
    public virtual void  UnEquip()
    {
        gameObject.SetActive(false);
    }

    public abstract void Attack();

    public abstract bool CanAttack();
}
