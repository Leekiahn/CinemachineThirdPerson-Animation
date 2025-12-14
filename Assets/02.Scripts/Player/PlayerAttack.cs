using UnityEngine;

public class PlayerAttack : CharacterAttack
{
    [Header("Player Stats")]
    public CharacterStatsData playerStatsData;

    [Header("Player Audio")]
    public AudioData playerAudioData;

    /// <summary>
    /// 적과의 충돌 처리
    /// </summary>
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Enemy에게 데미지 전달
            CharacterStats enemyStats = other.GetComponent<CharacterStats>();
            if (enemyStats != null && !enemyStats.isDead)
            {
                // 적 체력 감소
                enemyStats.TakeDamage(playerStatsData.attackDamage);

                // 공격 이펙트 생성
                SpawnAttackEffect(transform.position);

                // 적 피격 사운드 재생
                AudioClip hitClip = playerAudioData.GetRandomClip(playerAudioData.hitEnemySound);
                audioSource.PlayOneShot(hitClip);
            }
        }
    }

    /// <summary>
    /// 플레이어 공격 이펙트 생성 (1초 후 파괴)
    /// </summary>
    protected override void SpawnAttackEffect(Vector3 position)
    {
        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }
}
