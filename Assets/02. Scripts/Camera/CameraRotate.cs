using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    private CameraMode              _cameraMode = CameraMode.FPS;

    private Transform               _target;
    public float                    RotationSpeed = 150f;
    public float                    RotateDelayTime = 0.2f;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float                   _rotationX = 0;
    private float                   _rotationY = 0;

    // Recoil 설정
    [SerializeField] private float  recoilPower = 0.1f;
    [SerializeField] private float  recoilRecoverSpeed = 5f;

    private float                   _recoilOffsetY = 0f;   // 반동으로 추가되는 값


    private void Start()
    {
        CameraManager.Instance.OnChangeCameraMode += ChangeCameraMode;
    }

    private void ChangeCameraMode(CameraMode cameraMode, Transform target)
    {
        _cameraMode = cameraMode;
        _target = target;
    }

    public void Rotate()
    {
        // 구현 순서
        // 1. 마우스 입력을 받는다. (마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;

        // 반동 적용
        _rotationY += _recoilOffsetY;

        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        switch (_cameraMode)
        {
            case CameraMode.FPS:
                {
                    FPSRotate();
                    break;
                }
            case CameraMode.TPS:
                {
                    TPSRotate();
                    break;
                }
            case CameraMode.QuarterView:
                {
                    QuarterViewRotate();
                    break;
                }
        }

        // Recoil 보간 처리 (0으로 점점 줄이기)
        _recoilOffsetY = Mathf.MoveTowards(_recoilOffsetY, 0f, recoilRecoverSpeed * Time.deltaTime);
    }

    private void FPSRotate()
    {
        // 3. 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }

    private void TPSRotate()
    {
        // 3. 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        // transform.DORotate(new Vector3(-_rotationY, _rotationX, 0), 0.1f);
        // transform.forward = _target.position - transform.position;
        // transform.LookAt(_target);
    }

    private void QuarterViewRotate()
    {
        transform.LookAt(_target);
    }

    public void Recoil()
    {
        _recoilOffsetY += recoilPower;
    }
}
