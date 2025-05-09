using UnityEngine;

public class BombWeapon : AWeaponBase
{
    public Transform ThrowPosition;
    public float     MinThrowPower      = 5f;
    public float     MaxThrowPower      = 15f;
    public float     ChargeRate         = 10f;
    public int       MaxBombCount       = 3;

    private Bomb     _currentBomb       = null;

    private float    _currentThrowPower = 0f;
    private int      _currentBombCount;
    private bool     _isCharging        = false;

    private void Awake()
    {
        _currentBombCount = MaxBombCount;
    }

    private void Start()
    {
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
    }

    private void Update()
    {
        if (_currentBomb == null)
        {
            return;
        }

        _currentBomb.transform.position = ThrowPosition.position;
    }

    public override void Equip(Player player)
    {
        base.Equip(player);
        if (!CanAttack())
        {
            return;
        }

        _currentBomb = BombPool.Instance.Create(ThrowPosition.position);
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

        if (_currentBomb == null)
        {
            return;
        }

        Rigidbody bombRigidbody = _currentBomb.GetComponent<Rigidbody>();
        if (bombRigidbody == null)
        {
            return;
        }

        bombRigidbody.linearVelocity = Vector3.zero;
        bombRigidbody.angularVelocity = Vector3.zero;

        bombRigidbody.AddForce(Camera.main.transform.forward * _currentThrowPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);
        
        _currentBombCount--;
        _currentThrowPower = MinThrowPower;

        _currentBomb = null;

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.RefreshBombThrowPowerSlider(_currentThrowPower, MaxThrowPower);
    }

    public override void Attack() { } // 사용 안함

    public override bool CanAttack() => _currentBombCount > 0 && !_isCharging;
}
