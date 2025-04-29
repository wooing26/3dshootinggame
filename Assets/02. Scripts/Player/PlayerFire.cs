using System.Collections;
using UnityEngine;

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
    public GameObject                 UI_SniperZoom;
    public GameObject                 UI_CrossHair;

    [Header("조준 모드")]
    public float                      ZoomInSize             = 15f;
    public float                      ZoomOutSize            = 60f;
    private bool                      _zoomMode              = false;

    [Header("무기")]
    public Gun                        Gun;

    private void Awake()
    {
        _animationController = GetComponentInChildren<PlayerAnimationController>();
    }


    private void Start()
    {
        BombPool.Instance.SetPoolSize(MaxBombCount);

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.RefreshBulletText(Gun.CurrentAmmo, Gun.MaxAmmo);
        UIManager.Instance.SetReloadImageActive(false);
    }

    private void Update()
    {
        HandleZoom();
        HandleFire();
        HandleReload();
        // Ray : 레이저 (시작 위치, 방향)
        // RayCast : 레이저를 발사
        // RaycastHit : 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체
    }

    private void HandleZoom()
    {
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
        if (InputManager.Instance.GetMouseButton(0))
        {
            Gun.CancelReload();

            if (Gun.CanFire)
            {
                Gun.Attack();
                _animationController.PlayShotAnimation();
            }
            UIManager.Instance.RefreshBulletText(Gun.CurrentAmmo, Gun.MaxAmmo);
        }
    }

    private void HandleReload()
    {
        if (InputManager.Instance.GetKeyDown(KeyCode.R))
        {
            Gun.StartReload();
        }
    }

    private void HandleBombThrow()
    {
        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠
        //if (InputManager.Instance.GetMouseButton(1) && _currentBombCount > 0)
        //{
        //    _throwPower += ThrowPowerIncreaseRate * Time.deltaTime;
        //    if (_throwPower >= MaxThrowPower)
        //    {
        //        _throwPower = MaxThrowPower;
        //    }
        //    UIManager.Instance.RefreshBombThrowPowerSlider(_throwPower, MaxThrowPower);
        //}
        //else if (InputManager.Instance.GetMouseButtonUp(1))
        //{
        //    UseBomb();
        //    _throwPower = 1f;
        //    UIManager.Instance.RefreshBombThrowPowerSlider(_throwPower, MaxThrowPower);
        //}
    }

    private void UseBomb()
    {
        if (_currentBombCount <= 0)
        {
            return;
        }

        // 3. 발사 위치에 수류탄 생성하기
        Bomb bomb = BombPool.Instance.Create(transform.position);

        // 4. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기
        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        if (bombRigidbody == null)
        {
            Destroy(bomb);
            return;
        }

        bombRigidbody.linearVelocity = Vector3.zero;
        bombRigidbody.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);

        bombRigidbody.angularVelocity = Vector3.zero;
        bombRigidbody.AddTorque(Vector3.one);

        _currentBombCount--;
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
    }
}
