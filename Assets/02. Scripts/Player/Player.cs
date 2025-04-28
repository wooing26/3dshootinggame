using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [Header("체력 관련")]
    public int      MaxHealth = 100;
    private int     _currentHealth = 100;
    public int      CurrentHealth => _currentHealth;

    [Header("스태미나 관련")]
    public float    MaxStamina = 10f;
    private float   _currentStamina = 10f;
    public float    CurrentStamina => _currentStamina;

    public bool     IsRun = false;

    public float    IncreaseStaminaRate = 3f;

    public Action   OnChangePlayerStat;

    private void Awake()
    {
        _currentHealth = MaxHealth;
        _currentStamina = MaxStamina;
    }

    private void Update()
    {
        if (IsRun)
        {
            return;
        }
        RecoverStamina();
    }

    public void UseStamina(float amount)
    {
        _currentStamina -= amount;
        _currentStamina = Mathf.Max(_currentStamina, 0);

        OnChangePlayerStat?.Invoke();
    }

    private void RecoverStamina()
    {
        if (CurrentStamina < MaxStamina)
        {
            _currentStamina += IncreaseStaminaRate * Time.deltaTime;
            _currentStamina = Mathf.Min(CurrentStamina, MaxStamina);
            
            // 스테미나 UI 업데이트
            OnChangePlayerStat?.Invoke();
        }
    }

    [Header("피격 연출 관련")]
    public Image    BloodScreenImage;
    public float    BloodScreenTime = 1f;

    private Coroutine _bloodScreenCoroutine;
    public void TakeDamage(Damage damage)
    {
        _currentHealth -= damage.Value;
        
        if (_bloodScreenCoroutine != null)
        {
            StopCoroutine(_bloodScreenCoroutine);
        }
        _bloodScreenCoroutine = StartCoroutine(OnBloodScreen_Coroutine());

        OnChangePlayerStat?.Invoke();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator OnBloodScreen_Coroutine()
    {
        BloodScreenImage.gameObject.SetActive(true);

        float timer = BloodScreenTime;
        Color color = BloodScreenImage.color;
        color.a = 1;

        while (timer >= 0)
        {
            color.a = timer / BloodScreenTime;
            timer -= Time.deltaTime;
            BloodScreenImage.color = color;
            yield return null;
        }

        BloodScreenImage.gameObject.SetActive(false);
    }
}
