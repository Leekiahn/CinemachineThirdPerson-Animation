using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private AudioData healAudioData;

    [SerializeField] private float healAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false) return;

        CharacterStats characterStats = other.GetComponent<CharacterStats>();
        if (characterStats != null && !characterStats.isDead && characterStats.health < 100)
        {
            characterStats.HealHealth(healAmount);
            SFXManager.Instance.PlayOneShot(healAudioData.healSound);
            Debug.Log($"{other.gameObject.name} healed by {healAmount}. Current health: {characterStats.health}");
            Destroy(gameObject);
        }
    }
}
