using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyRootMotionController : CharacterRootMotionController
{
    [Header("Enemy Audio")]
    [SerializeField] private AudioData enemyAudioData;

    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown; // 공격 쿨다운
    [SerializeField] private float detectionRange; // 감지 범위

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius; // 순찰 반경
    [SerializeField] private float patrolWaitTime; // 순찰 지점 도착 후 대기 시간
    [SerializeField] private float reachDistance; // 목적지 도달 판정 거리

    private NavMeshAgent navMeshAgent;
    private bool hasAttacked = false;
    private float attackTimer = 0f;
    private Vector3 localVelocity;

    // 순찰 관련 변수
    private Vector3 patrolDestination;
    private float patrolWaitTimer = 0f;
    private bool isWaiting = false;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // NavMeshAgent 설정 - Root Motion 사용
        if (navMeshAgent != null)
        {
            navMeshAgent.updatePosition = false; // Root Motion이 위치 제어
            navMeshAgent.updateRotation = false; // 수동으로 회전 제어
        }

        // 초기 순찰 목적지 설정
        SetNewPatrolDestination();
    }

    protected override void Update()
    {
        base.Update();

        // NavMeshAgent와 Root Motion 위치 동기화
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.nextPosition = transform.position;
        }

        // 공격 쿨다운 타이머
        if (hasAttacked)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                hasAttacked = false;
                attackTimer = 0f;
            }
        }

        // 순찰 대기 타이머
        if (isWaiting)
        {
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer >= patrolWaitTime)
            {
                isWaiting = false;
                patrolWaitTimer = 0f;
                SetNewPatrolDestination();
            }
        }
    }

    protected override void UpdateMovement()
    {
        // 타겟이 없으면 플레이어 찾기
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
            else
            {
                return;
            }
        }

        if (navMeshAgent == null || !navMeshAgent.enabled)
        {
            return;
        }

        if (target.gameObject.GetComponent<CharacterStats>().isDead)
        {
            // 타겟이 죽었으면 순찰 모드로 전환
            PatrolMode();
            return;
        }

        // 타겟과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 공격 범위 안이면 공격
        if (distanceToTarget <= attackRange)
        {
            AttackMode();
        }
        // 감지 범위 안이면 추적
        else if (distanceToTarget <= detectionRange)
        {
            ChaseMode();
        }
        // 감지 범위 밖이면 순찰
        else
        {
            PatrolMode();
        }
    }

    /// <summary>
    /// 공격 모드
    /// </summary>
    private void AttackMode()
    {
        // NavMesh 완전 정지
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();

        // 이동 애니메이션 즉시 정지
        animator.SetFloat(hashMoveX, 0);
        animator.SetFloat(hashMoveY, 0);

        // 타겟 방향으로 회전
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0;

        if (directionToTarget.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // 쿨다운이 끝나면 공격
        if (IsGrounded() && !hasAttacked)
        {
            animator.SetTrigger(hashAttack);
            hasAttacked = true;
        }

        localVelocity = Vector3.zero;
    }

    /// <summary>
    /// 추적 모드
    /// </summary>
    private void ChaseMode()
    {
        // 순찰 대기 상태 해제
        isWaiting = false;
        patrolWaitTimer = 0f;

        // NavMeshAgent 활성화 및 목적지 설정
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);

        // NavMeshAgent의 desiredVelocity를 로컬 좌표로 변환
        localVelocity = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
        localVelocity.y = 0;

        // 애니메이터에 이동 값 전달 (정규화)
        if (navMeshAgent.speed > 0)
        {
            float normalizedX = localVelocity.x / navMeshAgent.speed;
            float normalizedZ = localVelocity.z / navMeshAgent.speed;

            animator.SetFloat(hashMoveX, normalizedX, smoothDampTime, Time.deltaTime);
            animator.SetFloat(hashMoveY, normalizedZ, smoothDampTime, Time.deltaTime);
        }

        // 이동 방향으로 회전
        if (navMeshAgent.desiredVelocity.magnitude > 0.1f)
        {
            Vector3 direction = navMeshAgent.desiredVelocity.normalized;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 순찰 모드
    /// </summary>
    private void PatrolMode()
    {
        // 대기 중이면 정지 상태 유지
        if (isWaiting)
        {
            navMeshAgent.isStopped = true;
            animator.SetFloat(hashMoveX, 0, smoothDampTime, Time.deltaTime);
            animator.SetFloat(hashMoveY, 0, smoothDampTime, Time.deltaTime);
            localVelocity = Vector3.zero;
            return;
        }

        // 목적지에 도착했는지 확인
        float distanceToDestination = Vector3.Distance(transform.position, patrolDestination);
        if (distanceToDestination <= reachDistance)
        {
            // 도착하면 대기 상태로 전환
            isWaiting = true;
            navMeshAgent.isStopped = true;
            animator.SetFloat(hashMoveX, 0, smoothDampTime, Time.deltaTime);
            animator.SetFloat(hashMoveY, 0, smoothDampTime, Time.deltaTime);
            localVelocity = Vector3.zero;
            return;
        }

        // 순찰 목적지로 이동
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(patrolDestination);

        // NavMeshAgent의 desiredVelocity를 로컬 좌표로 변환
        localVelocity = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
        localVelocity.y = 0;

        // 애니메이터에 이동 값 전달 (정규화)
        if (navMeshAgent.speed > 0)
        {
            float normalizedX = localVelocity.x / navMeshAgent.speed;
            float normalizedZ = localVelocity.z / navMeshAgent.speed;

            animator.SetFloat(hashMoveX, normalizedX, smoothDampTime, Time.deltaTime);
            animator.SetFloat(hashMoveY, normalizedZ, smoothDampTime, Time.deltaTime);
        }

        // 이동 방향으로 회전
        if (navMeshAgent.desiredVelocity.magnitude > 0.1f)
        {
            Vector3 direction = navMeshAgent.desiredVelocity.normalized;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 새로운 순찰 목적지 설정
    /// </summary>
    private void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Y축 고정

        NavMeshHit hit;
        // NavMesh 위의 유효한 위치 찾기
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolDestination = hit.position;
        }
        else
        {
            // 유효한 위치를 찾지 못하면 현재 위치 유지
            patrolDestination = transform.position;
        }
    }

    #region Animation Event Handlers - Audio

    /// <summary>
    /// 걷기 발소리 재생 Animation Event
    /// </summary>
    private void OnWalkFootStep()
    {
        if (localVelocity.magnitude > 0.1f && IsGrounded())
        {
            audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.walkFootStepSound));
        }
    }

    /// <summary>
    /// 달리기 발소리 재생 Animation Event
    /// </summary>
    private void OnSprintFootStep()
    {
        if (localVelocity.magnitude > 0.1f && IsGrounded())
        {
            audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.sprintFootStepSound));
        }
    }

    /// <summary>
    /// 착지 발소리 재생 Animation Event
    /// </summary>
    private void OnLandFootStep()
    {
        audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.landFootStepSound));
    }

    /// <summary>
    /// 착지 음성 재생 Animation Event
    /// </summary>
    private void OnLandVoice()
    {
        audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.landVoice));
    }

    /// <summary>
    /// 공격 음성 재생 Animation Event
    /// </summary>
    private void OnAttackVoice()
    {
        if (enemyAudioData != null && enemyAudioData.attackVoice != null && enemyAudioData.attackVoice.Length > 0)
        {
            audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.attackVoice));
        }
    }

    private void OnHitVoice()
    {
        audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.hitVoice));
    }

    private void OnDeadVoice()
    {
        audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.deadVoice));
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        // 순찰 반경 시각화 (파란색)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        // 감지 범위 시각화 (초록색)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위 시각화 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 현재 순찰 목적지 표시 (노란색)
        if (Application.isPlaying && patrolDestination != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(patrolDestination, 0.5f);
            Gizmos.DrawLine(transform.position, patrolDestination);
        }
    }
}