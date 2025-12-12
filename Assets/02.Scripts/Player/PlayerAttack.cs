using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerStatsData playerStatsData;
    private Collider attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<Collider>();
        if (attackCollider != null)
        {
            attackCollider.enabled = false; // 초기에는 비활성화
        }
    }

    /// <summary>
    /// Animation Event에서 호출: 공격 판정 시작
    /// </summary>
    public void EnableAttack()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }

    /// <summary>
    /// Animation Event에서 호출: 공격 판정 종료
    /// </summary>
    public void DisableAttack()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Damaged Enemy: " + playerStatsData.attackDamage);
            // TODO: Enemy에게 실제 데미지 전달
            // var enemy = other.GetComponent<Enemy>();
            // if (enemy != null) enemy.TakeDamage(playerStatsData.attackDamage);
        }
    }
}
