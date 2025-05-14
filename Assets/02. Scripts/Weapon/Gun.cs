using System.Collections;
using UnityEngine;

public class Gun : AWeaponBase
{
    public Transform      FirePosition;
    public Transform      RotatePivot;
    public ParticleSystem BulletEffect;
    public float          FireIntervalTime = 0.1f;
    public float          BulletLineLength = 15f;
    public int            MaxAmmo          = 50;

    private float         _fireTimer       = 0f;
    private int           _currentAmmo;
    private bool          _isReloading     = false;
    public float          ReloadTime       = 2f;
    private float         _reloadTimer     = 0f;
    private float         _lengthFromPivot = 0f;
    private LineRenderer  _bulletLineRenderer;

    public override bool  CanAttack()      => (_fireTimer >= FireIntervalTime && _currentAmmo > 0);
    public bool           IsReloading      => _isReloading;
    public int            CurrentAmmo      => _currentAmmo;

    private void Awake()
    {
        _bulletLineRenderer = GetComponent<LineRenderer>();

        _lengthFromPivot = Vector3.Distance(transform.position, RotatePivot.position);
        _currentAmmo = MaxAmmo;
    }

    public override void Equip(Player player)
    {
        base.Equip(player);

        _bulletLineRenderer.enabled = false;
        UIManager.Instance.RefreshBulletText(CurrentAmmo, MaxAmmo);
    }

    public override void UnEquip()
    {
        base.UnEquip();

        CancelReload();
    }

    private void Update()
    {
        if (CameraManager.Instance.CameraMode == CameraMode.QuarterView)
        {
            Vector2 mousePosition = InputManager.Instance.GetMousePositionFromCenter();
            if (mousePosition == Vector2.zero)
            {
                return;
            }
            
            RotatePivot.transform.forward = new Vector3(mousePosition.x, 0, mousePosition.y).normalized;
        }
        else
        {
            RotatePivot.transform.forward = Camera.main.transform.forward;
        }
        transform.position = RotatePivot.transform.position + RotatePivot.transform.forward * _lengthFromPivot;
        transform.forward = RotatePivot.transform.forward;

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= FireIntervalTime)
        {
            _fireTimer = FireIntervalTime;
        }

        if (_isReloading)
        {
            _reloadTimer += Time.deltaTime;
            UIManager.Instance.RefreshReloadImage(_reloadTimer, ReloadTime);

            if (_reloadTimer >= ReloadTime)
            {
                FinishReload();
            }
        }
    }

    public override void Attack()
    {
        if (_isReloading)
        {
            CancelReload();
        }

        // 총알 수 부족 or 총알 쿨 타임
        if (!CanAttack()) 
        { 
            return;
        }

        // 총알 줄이기, 쿨타임 측정
        _currentAmmo--;
        _fireTimer = 0f;

        // Ray : 레이저 (시작 위치, 방향)
        // RayCast : 레이저를 발사
        // RaycastHit : 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체

        // Ray를 이용한 피격 위치 생성
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        // 쿼터뷰면 캐릭터가 보고 있는 위치로 지정
        if (CameraManager.Instance.CameraMode == CameraMode.QuarterView)
        {
            ray = new Ray(FirePosition.position, FirePosition.forward);
        }

        // Ray를 발사하여 피격 정보 저장
        RaycastHit hitInfo;
        Vector3 hitPoint = FirePosition.position + ray.direction * BulletLineLength;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~(1 << LayerMask.NameToLayer("Player"))))
        {
            // 피격 위치 이펙트 재생
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal;
            BulletEffect.Play();

            hitPoint = hitInfo.point;

            // 데미지 주기
            IDamageable damagedEntity = hitInfo.collider.GetComponent<IDamageable>();
            if (damagedEntity != null)
            {
                Damage damage = new Damage
                {
                    Value = 10,
                    KnockBackPower = 10f,
                    From = gameObject,
                    HitPoint = hitInfo.point,
                    HitDirection = -hitInfo.normal
                };
                damagedEntity.TakeDamage(damage);
            }
        }

        if (isActiveAndEnabled)
        {
            StartCoroutine(ShotEffect(hitPoint));
        }
        
        CameraManager.Instance.Recoil();
        UIManager.Instance.RefreshBulletText(_currentAmmo, MaxAmmo);
    }

    public void StartReload()
    {
        _isReloading = true;
        _reloadTimer = 0f;
        UIManager.Instance.SetReloadImageActive(true);
        UIManager.Instance.RefreshReloadImage(_reloadTimer, ReloadTime);
    }

    private void CancelReload()
    {
        _isReloading = false;
        UIManager.Instance.SetReloadImageActive(false);
    }

    private void FinishReload()
    {
        _currentAmmo = MaxAmmo;
        _isReloading = false;
        UIManager.Instance.SetReloadImageActive(false);
        UIManager.Instance.RefreshBulletText(_currentAmmo, MaxAmmo);
    }

    private IEnumerator ShotEffect(Vector3 hitPoint)
    {
        _bulletLineRenderer.SetPosition(0, FirePosition.position);
        _bulletLineRenderer.SetPosition(1, hitPoint);
        _bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);
        _bulletLineRenderer.enabled = false;
    }
}
