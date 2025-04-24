using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 필요 속성
    // - 발사 위치
    public GameObject       FirePosition;
    // - 폭탄 프리팹
    public GameObject       BombPrefab;
    // - 던지는 힘
    public float            MaxThrowPower = 15f;
    public float            ThrowPowerIncreaseRate = 2f;
    private float           _throwPower = 1f;

    // - 폭탄 최대 개수
    public int              MaxBombCount = 3;
    private int             _currentBombCount = 3;

    // - 총알 최대 개수
    public int              MaxBulletCount = 50;
    private int             _currentBulletCount = 50;

    // - 총알 쿨타임
    public float            FireIntervalTime = 0.1f;
    private float           _fireTimer = 0f;

    // - 총알 재장전 타임
    public float            ReloadTime = 2f;
    private float           _relaodTimer = 0f;
    private bool            _isReload = false;

    // 목표 : 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다.
    public ParticleSystem   BulletEffect;

    private LineRenderer    _bulletLineRenderer;
    public float            BulletLineLength = 15f;

    private void Awake()
    {
        _bulletLineRenderer = GetComponent<LineRenderer>();
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        BombPool.Instance.SetPoolSize(MaxBombCount);

        _fireTimer = FireIntervalTime;

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
        UIManager.Instance.SetReloadImageActive(false);
    }

    private void Update()
    {
        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠
        if (Input.GetMouseButton(1) && _currentBombCount > 0)
        {
            _throwPower += ThrowPowerIncreaseRate * Time.deltaTime;
            if (_throwPower >= MaxThrowPower)
            {
                _throwPower = MaxThrowPower;
            }
            UIManager.Instance.RefreshBombThrowPowerSlider(_throwPower, MaxThrowPower);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            UseBomb();
            _throwPower = 1f;
            UIManager.Instance.RefreshBombThrowPowerSlider(_throwPower, MaxThrowPower);
        }


        // 총알 발사(레이저 방식)
        // 1. 왼쪽 버튼 입력 받기
        if (_fireTimer < FireIntervalTime)
        {
            _fireTimer += Time.deltaTime;
        }
        else if (Input.GetMouseButton(0))
        {
            FireBullet();
            _fireTimer = 0f;
            ResetReload();
        }

        // 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isReload = true;
            UIManager.Instance.SetReloadImageActive(true);
        }

        if (_isReload)
        {
            _relaodTimer += Time.deltaTime;
            UIManager.Instance.RefreshReloadImage(_relaodTimer, ReloadTime);
            if (_relaodTimer >= ReloadTime)
            {
                Reload();
                ResetReload();
            }
        }

        // Ray : 레이저 (시작 위치, 방향)
        // RayCast : 레이저를 발사
        // RaycastHit : 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체
    }

    private void UseBomb()
    {
        if (_currentBombCount <= 0)
        {
            return;
        }

        // 3. 발사 위치에 수류탄 생성하기
        Bomb bomb = BombPool.Instance.Create(FirePosition.transform.position);

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

    private void FireBullet()
    {
        if (_currentBulletCount <= 0)
        {
            return;
        }
        _currentBulletCount--;

        // 2. 레이를 생성하고 발사 위치와 진행 방향을 설정
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        // 3. 레이와 부딛힌 물체의 정보를 저장할 변수를 생성
        RaycastHit hitInfo = new RaycastHit();

        // 4. 레이를 발사한 다음,
        bool isHit = Physics.Raycast(ray, out hitInfo);
        Vector3 hitPosition = new Vector3();
        if (isHit)  // 데이터가 있다면(부딛혔다면)
        {
            // 피격 이펙트 생성(표시)
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal;    // 법선 벡터
            BulletEffect.Play();

            hitPosition = hitInfo.point;
            // 게임 수학 : 선형대수학(스칼라, 벡터, 행렬), 기하학(삼각함수)
            IDamageable damagedEntity = hitInfo.collider.GetComponent<IDamageable>();
            if (damagedEntity != null)
            {
                Damage damage = new Damage();
                damage.Value = 10;
                damage.KnockBackPower = 10f;
                damage.From = this.gameObject;

                damagedEntity.TakeDamage(damage);
            }
        }
        else
        {
            hitPosition = FirePosition.transform.position + Camera.main.transform.forward * BulletLineLength;
        }

        // 타격 지점 이펙트
        StartCoroutine(ShotEffect(hitPosition));

        // 총 반동
        CameraManager.Instance.Recoil();

        // UI 업데이트
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        _bulletLineRenderer.SetPosition(0, FirePosition.transform.position);

        _bulletLineRenderer.SetPosition(1, hitPosition);
        _bulletLineRenderer.enabled = true;


        yield return new WaitForSeconds(0.03f);
        _bulletLineRenderer.enabled = false;
    }

    private void Reload()
    {
        _currentBulletCount = MaxBulletCount;
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private void ResetReload()
    {
        _relaodTimer = 0f;
        _isReload = false;
        UIManager.Instance.SetReloadImageActive(false);
    }
}
