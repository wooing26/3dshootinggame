using UnityEngine;
using DG.Tweening;


public class CameraFollow : MonoBehaviour
{
    private CameraMode  _cameraMode = CameraMode.FPS;
    
    private Transform   _target;
    public float        TPSSpringArmLength = 15f;
    public float        MoveDelayTime = 0.2f;
    private bool        _isMoveEnd = false;

    private void Awake()
    {
        CameraManager.Instance.OnChangeCameraMode += ChangeCameraMode;
    }

    private void ChangeCameraMode(CameraMode cameraMode, Transform target)
    {
        _cameraMode = cameraMode;
        
        _target = target;
        _isMoveEnd = false;
        transform.DOMove(target.position, MoveDelayTime).OnComplete(() => { _isMoveEnd = true; });
    }

    private void LateUpdate()
    {
        switch (_cameraMode)
        {
            case CameraMode.FPS:
                {
                    FPSFollow();
                    break;
                }
            case CameraMode.TPS:
                {
                    TPSFollow();
                    break;
                }
        }
    }

    private void FPSFollow()
    {
        // 보간, smoothing 기법이 들어갈 예정
        // transform.DOMove(_target.position, MoveDelayTime);
        if (_isMoveEnd)
        {
            transform.position = _target.position;
        }
    }

    private void TPSFollow()
    {
        transform.DOMove(_target.position - _target.forward * TPSSpringArmLength, MoveDelayTime);
    }
}
