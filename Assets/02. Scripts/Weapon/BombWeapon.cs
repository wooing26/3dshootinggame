using UnityEngine;

public class BombWeapon : AWeaponBase
{
    public Transform ThrowPosition;
    public float     MinThrowPower      = 5f;
    public float     MaxThrowPower      = 15f;
    public float     ChargeRate         = 10f;
    public int       MaxBombCount       = 3;

    private float    _currentThrowPower = 0f;
    private int      _currentCount;
    private bool     _isCharging        = false;

    private void Awake()
    {
        _currentCount = MaxBombCount;

    }

    public override void Equip()
    {
        base.Equip();

    }
    public override void UnEquip()
    {
        base.UnEquip();

    }
    public void StartCharge()
    {
        if (!CanAttack())
        {
            return;
        }

        _isCharging = true;
        _currentThrowPower = MinThrowPower;
    }

    public void Charging()
    {
        if (!_isCharging)
        {
            return;
        }

        _currentThrowPower += ChargeRate * Time.deltaTime;
        if (_currentThrowPower > MaxThrowPower)
        {
            _currentThrowPower = MaxThrowPower;
        }

        UIManager.Instance.RefreshBombThrowPowerSlider(_currentThrowPower, MaxThrowPower);
    }

    public void Release()
    {
        if (!_isCharging || !CanAttack())
        {
            return;
        }

        _isCharging = false;

        Bomb bomb = BombPool.Instance.Create(ThrowPosition.position);
        if (bomb == null)
        {
            return;
        }

        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        if (bombRigidbody == null)
        {
            return;
        }

        bombRigidbody.linearVelocity = Vector3.zero;
        bombRigidbody.angularVelocity = Vector3.zero;

        bombRigidbody.AddForce(Camera.main.transform.forward * _currentThrowPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);
        
        _currentCount--;
        _currentThrowPower = MinThrowPower;

        UIManager.Instance.RefreshBombText(_currentCount, MaxBombCount);
        UIManager.Instance.RefreshBombThrowPowerSlider(_currentThrowPower, MaxThrowPower);
    }

    public override void Attack() { } // 사용 안함

    public override bool CanAttack() => _currentCount > 0;
}
