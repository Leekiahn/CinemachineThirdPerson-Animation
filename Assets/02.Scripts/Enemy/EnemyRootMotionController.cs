using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyRootMotionController : CharacterRootMotionController
{
    [Header("Enemy Audio")]
    [SerializeField] private PlayerAudioData enemyAudioData;

    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown; // 공격 쿨다운

    private NavMeshAgent navMeshAgent;
    private bool hasAttacked = false;
    private float attackTimer = 0f;
    private Vector3 localVelocity;

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

        // 타겟과의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // 공격 범위 안이면 공격
        if (distanceToTarget <= attackRange)
        {
            // NavMesh 완전 정지
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // 경로 초기화

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
        // 공격 범위 밖이면 추적
        else
        {
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
        audioSource.PlayOneShot(enemyAudioData.GetRandomClip(enemyAudioData.attackVoice));
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}