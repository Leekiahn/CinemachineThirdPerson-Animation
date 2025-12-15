using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] protected CharacterStats characterStats;

    [SerializeField] protected HealthBar healthBar;

    private void Awake()
    {
        if (characterStats == null)
        {
            characterStats = GetComponentInParent<CharacterStats>();
        }
    }

    protected virtual void OnEnable()
    {
        if (characterStats != null)
        {
            characterStats.onHealthChanged.AddListener(UpdateHealthBar);
        }
    }

    protected virtual void OnDisable()
    {
        if (characterStats != null)
        {
            characterStats.onHealthChanged.RemoveListener(UpdateHealthBar);
        }
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }
}
