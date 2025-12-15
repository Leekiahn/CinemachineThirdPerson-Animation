using UnityEngine;
using System.Collections;

/// <summary>
/// 캐릭터의 공격 판정을 처리하는 베이스 클래스
/// </summary>
public abstract class CharacterAttack : MonoBehaviour
{
    [SerializeField] protected AudioSource audioSource;
    protected Collider attackCollider;

    [SerializeField] protected GameObject attackEffect;

    protected virtual void Awake()
    {
        attackCollider = GetComponent<Collider>();

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    /// <summary>
    /// Animation Event에서 호출: 공격 판정 활성화
    /// </summary>
    public virtual void EnableAttack()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }

    /// <summary>
    /// Animation Event에서 호출: 공격 판정 비활성화
    /// </summary>
    public virtual void DisableAttack()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    /// <summary>
    /// 공격 판정 충돌 처리 (하위 클래스에서 구현)
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    protected abstract void OnTriggerEnter(Collider other);

    /// <summary>
    /// 플레이어 공격 이펙트 생성 (1초 후 비활성화)
    /// </summary>
    protected virtual void SpawnAttackEffect()
    {
        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }
    }
}
