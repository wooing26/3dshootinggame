using UnityEditor;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.

    public float RotationSpeed = 150f;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Update()
    {
        // 구현 순서
        // 1. 마우스 입력을 받는다. (마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        // Todo : 마우스 좌표계와 화면 좌표계의 차이점을 알고, 잘작동 하도록 아래 한줄의 코드를 잘 수정해 보기.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. 회전 방향으로 회전시킨다.
        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);

        //Vector3 dir = new Vector3(-mouseY, mouseX, 0);

        //// 3. 카메라를 그 방향으로 회전한다.
        //// 새로운 위치 = 현재 위치 + 이동 속도 * 시간
        //// 새로운 각도 = 현재 위치 + 회전 속도 * 시간
        //transform.eulerAngles = transform.eulerAngles + dir * RotationSpeed * Time.deltaTime;

        //// 회전의 상하 제한이 필요하다! (-90 ~ 90)
        //Vector3 rotation = transform.eulerAngles;
        //rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        //transform.eulerAngles = rotation;

        
    }
}
