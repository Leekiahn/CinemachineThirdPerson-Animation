using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : CharacterStats
{
    private NavMeshAgent navMeshAgent;
    private EnemyRootMotionController enemyRootMotionController;
    private EnemyFactory enemyFactory;

    [SerializeField] private float setActiveFalseTime; // 죽은 후 비활성화까지 시간
    private float currentDeathTimer;

    protected override void Awake()
    {
        base.Awake();
        enemyFactory = FindObjectOfType<EnemyFactory>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyRootMotionController = GetComponent<EnemyRootMotionController>();

        // 재활성화 시 컴포넌트 활성화
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.isStopped = false;
        }

        if (enemyRootMotionController != null)
        {
            enemyRootMotionController.enabled = true;
        }

        // 죽음 타이머 초기화
        currentDeathTimer = setActiveFalseTime;
    }

    private void Update()
    {
        // 죽은 상태라면 setActiveFalseTime 후에 풀로 반환
        if (isDead)
        {
            currentDeathTimer -= Time.deltaTime;
            if (currentDeathTimer <= 0f)
            {
                ReturnToPool();
            }
        }
    }

    protected override void Die()
    {
        base.Die();

        // 죽음 애니메이션
        animator.SetBool(hashDie, true);

        // NavMeshAgent 비활성화
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.enabled = false;
        }

        // 적 컨트롤러 비활성화
        if (enemyRootMotionController != null)
        {
            enemyRootMotionController.enabled = false;
        }

        // 타이머 리셋
        currentDeathTimer = setActiveFalseTime;
    }

    /// <summary>
    /// 적을 오브젝트 풀로 반환
    /// </summary>
    private void ReturnToPool()
    {
        if (enemyFactory != null)
        {
            enemyFactory.ReturnEnemyToPool(gameObject);
        }
        else
        {
            // Factory를 못 찾은 경우 일반 비활성화
            gameObject.SetActive(false);
        }
    }
}