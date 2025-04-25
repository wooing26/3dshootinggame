using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealth : MonoBehaviour
{
    public Slider HealthSlider;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.OnDamaged += UpdateHealthSlider;
    }

    private void UpdateHealthSlider(int health, int maxHealth)
    {
        HealthSlider.value = (float)health / _enemy.MaxHealth;
    }
}
