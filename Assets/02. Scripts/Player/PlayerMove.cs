using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터를 이동시키고 싶다.
    // 필요 속성:
    // - 이동 속력
    [Header("플레이어 움직임 데이터")]
    public PlayerMovementDataSO     _movementData;
    
    private float                   _currentSpeed = 7f;
    private bool                    _isRun = false;
    private bool                    _isRoll = false;
    private bool                    _canClimb = true;

    // 구르기 시간
    [SerializeField] private float  rollTime = 0.3f;
    private float                   _currentRollTimer = 0f;
    
    // 점프
    private int                     _currentjumpCount = 0;
    private const float             GRAVITY = -9.8f;    // 중력가속도
    private float                   _yVelocity = 0f;          // 중력 속도

    private CharacterController     _characterController;
    private Player                  _player;
    private Animator                _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
    }

    // 구현 순서 : 

    private void Update()
    {
        Jump();
        Move();   
    }

    private void Move()
    {
        // 구르기
        if (InputManager.Instance.GetKeyDown(KeyCode.E) && _player.CurrentStamina - _movementData.RollStamina >= 0)
        {
            _player.UseStamina(_movementData.RollStamina);
            _isRoll = true;
        }

        if (_isRoll)
        {
            Roll();
            return;
        }

        // 1. 키보드 입력을 받는다.
        float h = InputManager.Instance.GetAxis("Horizontal");
        float v = InputManager.Instance.GetAxis("Vertical");

        // 벽 타기
        if (_canClimb)
        {
            Climb(v);
        }
        
        // 달리기
        Run();

        // 2. 입력으로부터 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        // _animator.SetLayerWeight(2, _player.CurrentHealth / _player.MaxHealth);

        _animator.SetFloat("MoveForwardAmount", h);
        _animator.SetFloat("MoveRightAmount", v);

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
        if (InputManager.Instance.GetKey(KeyCode.LeftShift))
        {
            _isRun = true;
            _currentSpeed = _movementData.RunSpeed;
        }
        else
        {
            _isRun = false;
            _currentSpeed = _movementData.WalkSpeed;
        }
        
        _player.IsRun = _isRun;

        if (_isRun && _characterController.velocity != Vector3.zero)
        {
            _player.UseStamina(_movementData.DecreaseStaminaRate * Time.deltaTime);
            
            if (_player.CurrentStamina <= 0)
            {
                _isRun = false;
                _currentSpeed = _movementData.WalkSpeed;
            }
        }
    }

    private void Roll()
    {
        _currentRollTimer += Time.deltaTime;
        if (_currentRollTimer > rollTime)
        {
            _isRoll = false;
            _currentRollTimer = 0f;
            return;
        }
        _characterController.Move(transform.forward * _movementData.RollSpeed * Time.deltaTime);

        
    }

    private void Climb(float verticalInput)
    {
        if ((_characterController.collisionFlags & CollisionFlags.Sides) != 0)
        {
            if (verticalInput != 0)
            {
                _yVelocity = verticalInput * _movementData.ClimbSpeed * Time.deltaTime;
                _player.UseStamina(_movementData.ClimbStaminaRate * Time.deltaTime);
                if (_player.CurrentStamina <= 0)
                {
                    _canClimb = false;
                }
            }
        }
    }

    private void Jump()
    {
        // 캐릭터가 땅 위에 있다면...
        if (_characterController.isGrounded)
        {
            _currentjumpCount = 0;
            _canClimb = true;
        }

        // 3. 점프 구현
        if (InputManager.Instance.GetButtonDown("Jump") && _currentjumpCount < _movementData.MaxJumpCount)
        {
            _yVelocity = _movementData.JumpPower;
            _currentjumpCount++;
        }

        // 4. 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
    }
}
