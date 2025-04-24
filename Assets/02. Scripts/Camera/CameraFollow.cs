using UnityEngine;
using DG.Tweening;


public class CameraFollow : MonoBehaviour
{
    private CameraMode  _cameraMode = CameraMode.FPS;
    
    private Transform   _target;
    public float        TPSSpringArmLength = 5f;
    public Vector3      QuarterViewOffset = new Vector3(0, 15f, 0);
    public float        MoveDelayTime = 0.2f;
    private bool        _isMoveEnd = false;

    private void Start()
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

    public void Follow()
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
            case CameraMode.QuarterView:
                {
                    QuarterViewFollow();
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
        
        if (_isMoveEnd)
        {
            transform.position = _target.position - transform.forward * TPSSpringArmLength;
            // transform.DOMove(_target.position - transform.forward * TPSSpringArmLength, MoveDelayTime);
        }
    }

    private void QuarterViewFollow()
    {
        transform.position = _target.position + QuarterViewOffset;
    }
}
