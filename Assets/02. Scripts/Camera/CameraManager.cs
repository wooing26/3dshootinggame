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
    public Transform                        FPSTarget;

    [Header("TPS")]
    public Transform                        TPSTarget;

    [Header("QuarterView")]
    public Transform                        Player;

    public CameraMode                       CameraMode = CameraMode.FPS;
    public Action<CameraMode, Transform>    OnChangeCameraMode;

    private CameraRotate                    _cameraRotate;
    private CameraFollow                    _cameraFollow;

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
        CameraMode = cameraMode;
        OnChangeCameraMode?.Invoke(cameraMode, target);
    }

    public void Recoil()
    {
        _cameraRotate.Recoil();
    }
}
