using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전속도가 똑같아야 한다.

    private float _rotationX = 0;

    private void Update()
    {
        float mouseX = InputManager.Instance.GetAxis("Mouse X");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;

        if (CameraManager.Instance.CameraMode == CameraMode.QuarterView)
        {
            Vector2 mousePosition = InputManager.Instance.GetMousePositionFromCenter();

            transform.forward = new Vector3(mousePosition.x, 0, mousePosition.y).normalized;
            return;
        }

        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}
