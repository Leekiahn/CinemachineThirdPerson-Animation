using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    protected Animator animator;

    [SerializeField] protected CharacterStatsData characterStatsData;
    public float health { get; private set; }


    protected readonly int hashHit = Animator.StringToHash("Hit");
    protected readonly int hashDie = Animator.StringToHash("Die");

    public bool isDead { get; private set; } = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        health = characterStatsData.maxHealth;
    }

    public void HealHealth(float healAmount)
    {
        if (isDead) return;
        health += healAmount;
        if (health > characterStatsData.maxHealth)
        {
            health = characterStatsData.maxHealth;
        }
        Debug.Log($"{gameObject.name} healed by {healAmount}. Current health: {health}");
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger(hashHit);
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} has died.");
    }
}
