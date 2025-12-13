using UnityEngine;

public class PlayerStats : CharacterStats
{
    protected override void Die()
    {
        Debug.Log("Player has died. Game Over.");
    }
}
