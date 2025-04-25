using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int Health = 100;

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
