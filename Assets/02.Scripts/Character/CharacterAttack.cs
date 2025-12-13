using UnityEngine;

/// <summary>
/// 캐릭터의 공격 판정을 처리하는 베이스 클래스
/// </summary>
public abstract class CharacterAttack : MonoBehaviour
{
    protected AudioSource audioSource;
    protected Collider attackCollider;

    [SerializeField] protected GameObject attackEffect;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
    /// 공격 이펙트 생성
    /// </summary>
    /// <param name="position">이펙트 생성 위치</param>
    protected virtual void SpawnAttackEffect(Vector3 position)
    {
        if (attackEffect != null)
        {
            Instantiate(attackEffect, position, Quaternion.identity);
        }
    }
}
