using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

// 1. 상태를 열거형으로 정의한다.
public enum EEnemyState
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
    public EEnemyState                              ECurrentState = EEnemyState.Idle;
    private Dictionary<EEnemyState, AEnemyState>    _stateDictionary;
    private AEnemyState                             _currentStateBehaviour;

    // 필요 속성
    // 1. 플레이어(위치)
    private GameObject                              _player;                            // 플레이어
    public GameObject                               Player => _player;
    
    private CharacterController                     _characterController;               // 캐릭터 컨트롤러
    public CharacterController                      CharacterController => _characterController;
    
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
    

    public float                                    IdleWaitTime= 2f;

    public Transform[]                              PatrolTransforms;


    private void Awake()
    {
        _stateDictionary = new Dictionary<EEnemyState, AEnemyState>
        {
            { EEnemyState.Idle,      new EnemyIdleState()},
            { EEnemyState.Patrol,    new EnemyPatrolState()},
            { EEnemyState.Trace,     new EnemyTraceState()},
            { EEnemyState.Return,    new EnemyReturnState()},
            { EEnemyState.Attack,    new EnemyAttackState()},
            { EEnemyState.Damaged,   new EnemyDamagedState()},
            { EEnemyState.Die,       new EnemyDeadState()},
        };
}

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;


        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _startPosition = transform.position;

        ChangeState(EEnemyState.Idle);
    }

    private void Update()
    {
        _currentStateBehaviour?.Update();
    }

    public void ChangeState(EEnemyState enemyState)
    {
        if (_currentStateBehaviour != null)
        {
            _currentStateBehaviour.Exit();
        }

        ECurrentState = enemyState;
        _currentStateBehaviour = _stateDictionary[enemyState];
        _currentStateBehaviour.Enter(this);
    }

    public void TakeDamage(Damage damage)
    {
        // 사망했거나 공격받고 있는 중이면...
        if (ECurrentState == EEnemyState.Damaged || ECurrentState == EEnemyState.Die)
        {
            return;
        }

        Health -= damage.Value;

        if (Health <= 0)
        {

            Debug.Log($"상태 전환 : {ECurrentState} -> Die");

            ChangeState(EEnemyState.Damaged);
            return;
        }

        Vector3 dir = (transform.position - damage.From.transform.position).normalized;

        EnemyDamagedState damagedState = (EnemyDamagedState)_stateDictionary[EEnemyState.Damaged];
        if (damagedState != null)
        {
            damagedState.SetKnockBack(dir, damage.KnockBackPower);
        }
        
        Debug.Log($"상태 전환 : {ECurrentState} -> Damaged");

        ChangeState(EEnemyState.Damaged);
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
        ChangeState(EEnemyState.Trace);
    }
}
