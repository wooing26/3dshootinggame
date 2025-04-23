using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    private CameraMode  _cameraMode = CameraMode.FPS;

    private Transform   _target;
    public float        RotationSpeed = 150f;
    public float        RotateDelayTime = 0.2f;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float       _rotationX = 0;
    private float       _rotationY = 0;

    private float[]     _mouseMove = { 0, 0 };

    private void Awake()
    {
        CameraManager.Instance.OnChangeCameraMode += ChangeCameraMode;
    }

    private void ChangeCameraMode(CameraMode cameraMode, Transform target)
    {
        _cameraMode = cameraMode;
        _target = target;
    }

    private void LateUpdate()
    {
        // 구현 순서
        // 1. 마우스 입력을 받는다. (마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mouseMove[0] = mouseX;
        _mouseMove[1] = mouseY;
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
        }
    }

    private void FPSRotate()
    {
        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += _mouseMove[0] * RotationSpeed * Time.deltaTime;
        _rotationY += _mouseMove[1] * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }

    private void TPSRotate()
    {
        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        _rotationX += _mouseMove[0] * RotationSpeed * Time.deltaTime;
        _rotationY += _mouseMove[1] * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 회전 방향으로 회전시킨다.
        _target.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        transform.forward = _target.position - transform.position;
    }
}
