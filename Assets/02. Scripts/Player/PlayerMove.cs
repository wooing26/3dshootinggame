using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터를 이동시키고 싶다.
    // 필요 속성:
    // - 이동 속력
    [Header("이동 속력")]
    [SerializeField] private float  walkSpeed = 7f;
    [SerializeField] private float  runSpeed = 12f;
    private float                   _currentSpeed = 7f;
    private bool                    _isRun = false;

    [Header("스테미너")]
    public Slider                   StaminaSlider;
    public float                    MaxStamina = 10f;
    private float                   _stamina = 10f;
    [SerializeField] private float  increaseStaminaRate = 3f;
    [SerializeField] private float  decreaseStaminaRate = 1f;
    
    [Header("점프")]
    [SerializeField] private float  jumpPower = 10f;

    private const float             GRAVITY = -9.8f;    // 중력가속도
    private float                   _yVelocity = 0f;          // 중력 속도
    private bool                    _isJumping = false;



    private CharacterController     _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // 구현 순서 : 
    
    private void Update()
    {
        Jump();
        Move();   
    }

    private void Move()
    {
        // 1. 키보드 입력을 받는다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Run();

        // 2. 입력으로부터 방향을 설정한다.
        //Vector3 dir = h * transform.right + v * transform.forward;
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 2-1. 메인 카메라를 기준으로 방향으로 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = _yVelocity;

        // 5. 방향에 따라 플레이어를 이동한다.
        _characterController.Move(dir * _currentSpeed * Time.deltaTime);
    }

    private void Run()
    {
        // 달리기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isRun = true;
            _currentSpeed = runSpeed;
        }
        else
        {
            _isRun = false;
            _currentSpeed = walkSpeed;
        }

        if (_isRun)
        {
            _stamina -= decreaseStaminaRate * Time.deltaTime;
            if (_stamina <= 0)
            {
                _stamina = 0f;
                _isRun = false;
                _currentSpeed = walkSpeed;
            }
        }
        else
        {
            _stamina += increaseStaminaRate * Time.deltaTime;
            if (_stamina >= MaxStamina)
            {
                _stamina = MaxStamina;
            }
        }

        StaminaSlider.value = _stamina / MaxStamina;
    }

    private void Jump()
    {
        // 캐릭터가 땅 위에 있다면...
        if (_characterController.isGrounded)
        {
            _isJumping = false;
        }

        // 3. 점프 구현
        if (Input.GetButtonDown("Jump") && _isJumping == false)
        {
            _yVelocity = jumpPower;

            _isJumping = true;
        }

        // 4. 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
    }
}
