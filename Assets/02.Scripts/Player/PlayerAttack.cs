using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private AudioSource audioSource;
    private Collider attackCollider;

    public PlayerStatsData playerStatsData;
    public PlayerAudioData playerAudioData;

    [SerializeField] private GameObject attackEffect;

    private void Awake()
    {
        attackCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
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

            GameObject effect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // 이펙트 오브젝트 파괴

            AudioClip hitClip = playerAudioData.GetRandomClip(playerAudioData.hitEnemySound);
            audioSource.PlayOneShot(hitClip);
        }
    }
}
