using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : CharacterStats
{
    private NavMeshAgent navMeshAgent;
    private EnemyRootMotionController enemyRootMotionController;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyRootMotionController = GetComponent<EnemyRootMotionController>();
    }

    protected override void Die()
    {
        base.Die();
        animator.SetBool(hashDie, true);
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }

        if (enemyRootMotionController != null)
        {
            enemyRootMotionController.enabled = false;
        }

        Destroy(gameObject, 20f);
    }
}
