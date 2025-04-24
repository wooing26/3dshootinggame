using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

// 1. 상태를 열거형으로 정의한다.
public enum EnemyState
{
    Idle,
    Patrol,
    Trace,
    Return,
    Attack,
    Damaged,
    Die
}
public class Enemy : MonoBehaviour, IDamageable
{
    // 2. 현재 상태를 지정한다.
    public EnemyState                               CurrentState = EnemyState.Idle;
    private Dictionary<EnemyState, AEnemyState>     _stateDictionary;
    private AEnemyState                             _currentState;

    // 필요 속성
    // 1. 플레이어(위치)
    private GameObject                              _player;                            // 플레이어
    public GameObject                               Player => _player;
    private CharacterController                     _characterController;               // 캐릭터 컨트롤러
    
    private NavMeshAgent                            _agent;                             // 네비메시 에이전트
    public NavMeshAgent                             Agent => _agent;
    private Vector3                                 _startPosition;                     // 시작 위치
    public Vector3                                  StartPosition => _startPosition;

    // 2. Distance
    public float                                    FindDistance = 7f;                  // 탐지 범위
    public float                                    ReturnDistance = 1f;                // 복귀 범위
    public float                                    AttackDistance = 2.5f;              // 공격 범위
    public float                                    MoveSpeed = 3.3f;                   // 움직임 속력

    public float                                    AttackCoolTime = 2f;                // 공격 쿨타임

    public int                                      Health = 100;                       // 체력
    public float                                    DamagedTime = 0.5f;
    public float                                    DeathTime = 2f;
    private Vector3                                 _knockBackDir;
    private float                                   _knockBackPower;

    public float                                    IdleWaitTime= 2f;
    private float                                   _idleTimer = 0f;

    public Transform[]                              PatrolTransforms;


    private void Awake()
    {

    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;


        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _startPosition = transform.position;
    }

    private void Update()
    {

        _currentState?.Update();
    }

    public void ChangeState(EnemyState enemyState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        
        _currentState = _stateDictionary[enemyState];
        _currentState.Enter(this);
    }

    public void TakeDamage(Damage damage)
    {
        // 사망했거나 공격받고 있는 중이면...
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;

        if (Health <= 0)
        {

            Debug.Log($"상태 전환 : {CurrentState} -> Die");

            ChangeState(EnemyState.Damaged);
            StartCoroutine(Die_Coroutine());
            return;
        }

        Vector3 dir = (transform.position - damage.From.transform.position).normalized;
        _knockBackPower = damage.KnockBackPower;
        _knockBackDir = dir;
        Debug.Log($"상태 전환 : {CurrentState} -> Damaged");

        ChangeState(EnemyState.Damaged);
        StartCoroutine(Damaged_Coroutine());
    }

    // 3. 상태 함수들을 구현한다.
    private IEnumerator Damaged_Coroutine()
    {
        // 행동 : 공격 당한다.
        //_damagedTimer += Time.deltaTime;
        //if (_damagedTimer >= DamagedTime)
        //{
        //    _damagedTimer = 0f;
        //    Debug.Log("상태전환 : Damaged -> Trace");
        //    CurrentState = EnemyState.Trace;
        //}

        // 코루틴 방식으로 변경
        _agent.isStopped = true;
        _agent.ResetPath();
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log("상태전환 : Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        // 행동 : 죽는다.
        yield return new WaitForSeconds(DeathTime);
        gameObject.SetActive(false);
    }
}
