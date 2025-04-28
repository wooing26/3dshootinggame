using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10f;

    private void LateUpdate()
    {
        transform.position = Target.position + Vector3.up * YOffset;
        
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;

        if (CameraManager.Instance.CameraMode == CameraMode.QuarterView)
        {
            newEulerAngles.y = 0;
        }

        transform.eulerAngles = newEulerAngles;
    }
}
