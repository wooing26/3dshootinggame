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
    public Transform FPSTarget;

    [Header("TPS")]
    public Transform TPSTarget;

    public CameraMode CameraMode = CameraMode.FPS;
    public Action<CameraMode, Transform> OnChangeCameraMode;

    private void Start()
    {
        ChangeCameraMode(CameraMode.FPS, FPSTarget);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ChangeCameraMode(CameraMode.FPS, FPSTarget);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ChangeCameraMode(CameraMode.TPS, TPSTarget);
        }
    }

    private void ChangeCameraMode(CameraMode cameraMode, Transform target)
    {
        CameraMode = cameraMode;
        OnChangeCameraMode?.Invoke(cameraMode, target);
    }
}
