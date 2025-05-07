using System.Collections.Generic;
using UnityEngine;
using VInspector;

public enum WeaponType
{
    Gun,
    Knife,
    Bomb
}

public class PlayerFire : MonoBehaviour
{
    // 필요 속성
    // - 던지는 힘
    [Header("수류탄")]
    public float                      MaxThrowPower          = 15f;
    public float                      ThrowPowerIncreaseRate = 2f;
    private float                     _throwPower            = 1f;

    // - 폭탄 최대 개수
    public int                        MaxBombCount           = 3;
    private int                       _currentBombCount      = 3;

    // 목표 : 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다.
    private PlayerAnimationController _animationController;

    [Header("UI")]
    public GameObject                                           UI_SniperZoom;
    public GameObject                                           UI_CrossHair;

    [Header("조준 모드")]
    public float                                                ZoomInSize             = 15f;
    public float                                                ZoomOutSize            = 60f;
    private bool                                                _zoomMode              = false;


    [System.Serializable]
    public class WeaponTypeWeaponPair
    {
        public WeaponType  Type;
        public AWeaponBase Weapon;
    }

    [Header("무기")]
    [SerializeField] private List<WeaponTypeWeaponPair>         weaponList;
    private Dictionary<WeaponType, AWeaponBase>                     _weaponDictionary = new Dictionary<WeaponType, AWeaponBase>();
    private AWeaponBase                                             _currentWeapon;
    private WeaponType                                          _currentWeaponType = WeaponType.Gun;
    

    private void Awake()
    {
        _animationController = GetComponentInChildren<PlayerAnimationController>();

        foreach (var weapon in weaponList)
        {
            _weaponDictionary[weapon.Type] = weapon.Weapon;
        }
    }


    private void Start()
    {
        BombPool.Instance.SetPoolSize(MaxBombCount);

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.SetReloadImageActive(false);

        EquipWeapon(WeaponType.Gun);
    }

    private void Update()
    {
        HandleZoom();
        HandleFire();
        HandleReload();
        HandleWeaponSwitch();
        HandleBombThrow();
    }

    private void HandleZoom()
    {
        if (_currentWeaponType != WeaponType.Gun)
        {
            return;
        }

        if (InputManager.Instance.GetMouseButtonDown(1))
        {
            _zoomMode = !_zoomMode;
            UI_SniperZoom.SetActive(_zoomMode);
            UI_CrossHair.SetActive(!_zoomMode);

            Camera.main.fieldOfView = _zoomMode ? ZoomInSize : ZoomOutSize;
        }
    }

    private void HandleFire()
    {
        if (InputManager.Instance.GetMouseButton(0) && _currentWeapon.CanAttack())
        {
            _currentWeapon.Attack();
            _animationController.PlayShotAnimation();   
        }
    }

    private void HandleReload()
    {
        if (_currentWeaponType != WeaponType.Gun)
        {
            return;
        }

        if (InputManager.Instance.GetKeyDown(KeyCode.R))
        {
            Gun gun = (Gun)_currentWeapon;
            gun.StartReload();
        }
    }

    private void HandleWeaponSwitch()
    {
        if (InputManager.Instance.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(WeaponType.Gun);
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponType.Knife);
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(WeaponType.Bomb);
        }
    }

    private void EquipWeapon(WeaponType weaponType)
    {
        _currentWeaponType = weaponType;

        // 무기 GameObject On/Off
        if (_currentWeapon != null)
        {
            _currentWeapon.UnEquip();
        }

        _currentWeapon = _weaponDictionary[weaponType];
        _currentWeapon.Equip();

        // 애니메이터에 무기 타입 전달
        _animationController.ChangeWeaponAnimation(weaponType);

        // zoom 모드 해제
        _zoomMode = false;
        UI_SniperZoom.SetActive(_zoomMode);
        UI_CrossHair.SetActive(!_zoomMode);

        Camera.main.fieldOfView = ZoomOutSize;
    }

    private void HandleBombThrow()
    {
        if (_currentWeaponType != WeaponType.Bomb)
        {
            return;
        }

        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠
        BombWeapon bombWeapon = (BombWeapon)_currentWeapon;
        if (bombWeapon == null) return;

        if (InputManager.Instance.GetMouseButtonDown(0))
        {
            bombWeapon.StartCharge();
        }
        else if (InputManager.Instance.GetMouseButton(0))
        {
            bombWeapon.Charging();
        }
        else if (InputManager.Instance.GetMouseButtonUp(0))
        {
            bombWeapon.Release();
        }
    }

    
}
