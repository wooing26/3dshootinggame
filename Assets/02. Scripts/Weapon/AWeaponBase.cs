using UnityEngine;

public abstract class AWeaponBase : MonoBehaviour
{
    protected Player _player = null;

    public virtual void  Equip(Player player)
    {
        if (_player == null)
        {
            _player = player;
        }
        gameObject.SetActive(true);
    }
    public virtual void  UnEquip()
    {
        gameObject.SetActive(false);
    }

    public abstract void Attack();

    public abstract bool CanAttack();
}
