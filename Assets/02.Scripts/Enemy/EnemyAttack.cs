using UnityEngine;

public class EnemyAttack : CharacterAttack
{
    [Header("Enemy Stats")]
    public CharacterStatsData enemyStatsData;

    [Header("Enemy Audio")]
    public AudioData enemyAudioData;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            CharacterStats playerStats = other.GetComponent<CharacterStats>();
            if (playerStats != null && !playerStats.isDead)
            {
                // 플레이어에게 데미지 적용
                playerStats.TakeDamage(enemyStatsData.attackDamage);

                // 공격 이펙트 생성
                SpawnAttackEffect();

                // 플레이어 타격 사운드 재생
                AudioClip hitClip = enemyAudioData.GetRandomClip(enemyAudioData.hitEnemySound);
                audioSource.PlayOneShot(hitClip);
            }
        }
    }
}
