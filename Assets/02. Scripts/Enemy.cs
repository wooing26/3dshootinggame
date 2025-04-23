using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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
public class Enemy : MonoBehaviour
{
    // 2. 현재 상태를 지정한다.
    public EnemyState               CurrentState = EnemyState.Idle;

    // 필요 속성
    // 1. 플레이어(위치)
    private GameObject              _player;                            // 플레이어
    private CharacterController     _characterController;               // 캐릭터 컨트롤러
    private Vector3                 _startPosition;                     // 시작 위치

    // 2. Distance
    public float                    FindDistance = 7f;                  // 탐지 범위
    public float                    ReturnDistance = 1f;                // 복귀 범위
    public float                    AttackDistance = 2.5f;              // 공격 범위
    public float                    MoveSpeed = 3.3f;                   // 움직임 속력

    public float                    AttackCoolTime = 2f;                // 공격 쿨타임
    public float                    _attackTimer = 0f;                  //  ㄴ 체크기

    public int                      Health = 100;                       // 체력
    public float                    DamagedTime = 0.5f;
    public float                    DeathTime = 2f;
    private Vector3                 _knockBackDir;
    private float                   _knockBackPower;

    public float                    IdleWaitTime= 2f;
    private float                   _idleTimer = 0f;

    public Transform[]              PatrolTransforms;
    private List<Vector3>           _patrolPositions;
    private int                     _patrolIndex = 0;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _startPosition = transform.position;

        _patrolPositions = new List<Vector3>(PatrolTransforms.Length);
        foreach(Transform transform in PatrolTransforms)
        {
            _patrolPositions.Add(transform.position);
        }
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                Idle();
                break;
            }
            case EnemyState.Patrol:
            {
                Patrol();
                break;
            }
            case EnemyState.Trace:
            {
                Trace();
                break;
            }
            case EnemyState.Return:
            {
                Return();
                break;
            }
            case EnemyState.Attack:
            {
                Attack();
                break;
            }
            case EnemyState.Damaged:
            {
                Damaged();
                break;
            }
        }

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

            CurrentState = EnemyState.Die;
            StartCoroutine(Die_Coroutine());
            return;
        }

        Vector3 dir = (transform.position - damage.From.transform.position).normalized;
        _knockBackPower = damage.KnockBackPower;
        _knockBackDir = dir;
        Debug.Log($"상태 전환 : {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }

    // 3. 상태 함수들을 구현한다.
    private void Idle()
    {
        // 행동 : 가만히 있는다.
        if (Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
            _idleTimer = 0f;
            return;
        }

        _idleTimer += Time.deltaTime;
        if (_idleTimer >= IdleWaitTime)
        {
            Debug.Log("상태전환 : Idle -> Patrol");
            CurrentState = EnemyState.Patrol;
            _idleTimer = 0f;
            return;
        }
    }

    private void Patrol()
    {
        // 순찰 구역 가면 다시 Idle 상태로 변환
        if (Vector3.Distance(transform.position, _patrolPositions[_patrolIndex]) <= ReturnDistance)
        {
            Debug.Log("상태전환 : Patrol -> Idle");
            transform.position = _patrolPositions[_patrolIndex];
            CurrentState = EnemyState.Idle;
            _patrolIndex = (_patrolIndex + 1) % _patrolPositions.Count;
            return;
        }

        // 중간에 플레이어 찾으면 Trace로 변경
        if (Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            Debug.Log("상태전환 : Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // 순찰 구역으로 이동
        Vector3 dir = (_patrolPositions[_patrolIndex] - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Trace()
    {
        // 전이 : 공격 범위 만큼 멀어지면 -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) > FindDistance)
        {
            Debug.Log("상태전환 : Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // 전이 : 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 행동 : 플레이어를 추적한다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // 전이 : 시작 위치와 가까워 지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= ReturnDistance)
        {
            Debug.Log("상태전환 : Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // 전이 : 시작 위치와 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            Debug.Log("상태전환 : Return -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // 행동 :처음 자리로 되돌아간다.
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // 전이 : 공격 범위 만큼 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) > AttackDistance)
        {
            Debug.Log("상태전환 : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            return;
        }

        // 행동 : 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCoolTime)
        {
            Debug.Log("Attack");
            _attackTimer = 0f;
        }
    }

    private void Damaged()
    {
        _characterController.Move(_knockBackDir * _knockBackPower * Time.deltaTime);
    }

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
