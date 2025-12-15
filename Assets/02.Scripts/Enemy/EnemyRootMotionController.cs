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
    [SerializeField] private float attackExitRange; // 공격 상태를 벗어나는 거리
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

    // 공격 상태 추적
    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updatePosition = false; // Root Motion과 충돌 방지
        navMeshAgent.updateRotation = false; // Root Motion과 충돌 방지
    }

    protected override void Update()
    {
        base.Update();

        navMeshAgent.nextPosition = transform.position;

        // localVelocity 업데이트 추가
        localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
    }

    private void OnEnable()
    {
        // 플레이어 찾기
        FindPlayer();
    }

    protected override void UpdateMovement()
    {
        if (target == null)
        {
            isAttacking = false;
            hasAttacked = false;
            Patrol();
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // detectionRange 밖에 있거나 타겟이 죽었으면 순찰
        if (distanceToTarget > detectionRange || target.gameObject.GetComponent<CharacterStats>().isDead)
        {
            isAttacking = false;
            hasAttacked = false;
            Patrol();
            return;
        }

        // 공격 모드 진입/유지 로직
        if (distanceToTarget <= attackRange)
        {
            isAttacking = true;
        }
        else if (distanceToTarget > attackExitRange)
        {
            isAttacking = false;
            hasAttacked = false;
        }

        // 공격 모드일 때
        if (isAttacking)
        {
            Attack();
            return;
        }

        // 감지 범위 내에 있으면 추적
        if (distanceToTarget <= detectionRange)
        {
            Chase();
        }
    }

    /// <summary>
    /// Player 찾기
    /// </summary>
    private void FindPlayer()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");

        if (PlayerObject != null)
        {
            target = PlayerObject.transform;
        }
        else
        {
            target = null;
        }
    }

    /// <summary>
    /// 순찰 로직
    /// </summary>
    private void Patrol()
    {
        // 대기 중
        if (isWaiting)
        {
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer >= patrolWaitTime)
            {
                isWaiting = false;
                patrolWaitTimer = 0f;
            }

            animator.SetFloat(hashMoveY, 0f);
            animator.SetFloat(hashMoveX, 0f);
            animator.SetBool(hashIsSprinting, false);
            return;
        }

        // 목적지 도착 -> 대기 시작
        if (patrolDestination != Vector3.zero &&
            Vector3.Distance(transform.position, patrolDestination) <= reachDistance)
        {
            isWaiting = true;
            patrolDestination = Vector3.zero;
            return;
        }

        // 새 목적지 설정
        if (patrolDestination == Vector3.zero)
        {
            patrolDestination = GetRandomPointOnNavMesh(transform.position, patrolRadius);
            navMeshAgent.SetDestination(patrolDestination);
        }

        // 이동
        if (navMeshAgent.hasPath)
        {
            Vector3 direction = (navMeshAgent.steeringTarget - transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                Vector3 localMove = transform.InverseTransformDirection(direction);
                animator.SetFloat(hashMoveY, localMove.z, smoothDampTime, Time.deltaTime);
                animator.SetFloat(hashMoveX, localMove.x, smoothDampTime, Time.deltaTime);
                animator.SetBool(hashIsSprinting, true);
            }
        }
    }
    /// <summary>
    /// NavMesh 상의 랜덤 위치 반환
    /// </summary>
    /// <param name="currentPosition"> 적의 현재 위치</param>
    /// <param name="radius"> 탐색 반경 </param>
    /// <returns> 유효한 NavMesh 위치, 실패 시 currentPosition 반환</returns>
    private Vector3 GetRandomPointOnNavMesh(Vector3 currentPosition, float radius)
    {
        for (int i = 0; i < 30; i++) // 최대 30번 시도
        {
            Vector3 randomPoint = currentPosition + Random.insideUnitSphere * radius;
            randomPoint.y = 0f;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return currentPosition; // 실패 시 현재 위치 반환
    }


    /// <summary>
    /// 추적 로직
    /// </summary>
    private void Chase()
    {
        navMeshAgent.SetDestination(target.position);
        animator.SetBool(hashIsSprinting, false);
        if (navMeshAgent.hasPath)
        {
            Vector3 direction = (navMeshAgent.steeringTarget - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
                Vector3 localMove = transform.InverseTransformDirection(direction);
                animator.SetFloat(hashMoveY, localMove.z, smoothDampTime, Time.deltaTime);
                animator.SetFloat(hashMoveX, localMove.x, smoothDampTime, Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 공격 로직
    /// </summary>
    private void Attack()
    {
        navMeshAgent.ResetPath();

        // 타겟 방향으로 회전 (공격 중에도 타겟을 바라봄)
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0f;
        if (directionToTarget != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(directionToTarget),
                Time.deltaTime * 10f
            );
        }

        // 이동 애니메이션 정지
        animator.SetFloat(hashMoveY, 0f);
        animator.SetFloat(hashMoveX, 0f);
        animator.SetBool(hashIsSprinting, false);

        // 공격 쿨다운 처리
        if (!hasAttacked)
        {
            // 즉시 첫 공격 실행
            animator.SetTrigger(hashAttack);
            hasAttacked = true;
            attackTimer = 0f;
        }
        else
        {
            // 쿨다운 대기
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                animator.SetTrigger(hashAttack);
                attackTimer = 0f;
            }
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

        // 공격 유지 범위 시각화 (주황색)
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, attackExitRange);

        // 현재 순찰 목적지 표시 (노란색)
        if (Application.isPlaying && patrolDestination != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(patrolDestination, 0.5f);
            Gizmos.DrawLine(transform.position, patrolDestination);
        }
    }
}