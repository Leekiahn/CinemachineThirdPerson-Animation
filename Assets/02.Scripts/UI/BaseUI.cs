using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [SerializeField] protected CharacterStats characterStats;
    [SerializeField] protected HealthBar healthBar;

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

    protected virtual void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }
}
