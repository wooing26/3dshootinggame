using System;
using UnityEngine;
public enum CameraMode
{
    FPS,
    TPS,
    QuarterView
}

public class CameraManager : SingletonBehaviour<CameraManager>
{
    [Header("FPS")]
    public Transform                     FPSTarget;
    public LayerMask                     InVisibleInFPS;

    [Header("TPS")]
    public Transform                     TPSTarget;

    [Header("QuarterView")]
    public Transform                     Player;

    [Header("전체 카메라 설정")]
    public LayerMask                     DefaultVisibleLayer;
    public CameraMode                    CameraMode = CameraMode.FPS;
    public Action<CameraMode, Transform> OnChangeCameraMode;

    private CameraRotate                 _cameraRotate;
    private CameraFollow                 _cameraFollow;

    private void Start()
    {
        _cameraRotate = GetComponent<CameraRotate>();
        _cameraFollow = GetComponent<CameraFollow>();
        ChangeCameraMode(CameraMode.FPS, FPSTarget);
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(KeyCode.Alpha8))
        {
            ChangeCameraMode(CameraMode.FPS, FPSTarget);
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha9))
        {
            ChangeCameraMode(CameraMode.TPS, TPSTarget);
        }
        else if (InputManager.Instance.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeCameraMode(CameraMode.QuarterView, Player);
        }

        
    }

    private void LateUpdate()
    {
        _cameraRotate.Rotate();
        _cameraFollow.Follow();
    }

    private void ChangeCameraMode(CameraMode cameraMode, Transform target)
    {
        if (cameraMode == CameraMode.FPS)
        {
            Camera.main.cullingMask &= ~InVisibleInFPS.value;
        }
        else
        {
            Camera.main.cullingMask = DefaultVisibleLayer.value;
        }

        if (cameraMode == CameraMode.QuarterView)
        {
            UIManager.Instance.SetCrosshairImageActive(false);
        }
        else
        {
            UIManager.Instance.SetCrosshairImageActive(true);
        }

        CameraMode = cameraMode;
        OnChangeCameraMode?.Invoke(cameraMode, target);
    }

    public void Recoil()
    {
        _cameraRotate.Recoil();
    }
}
