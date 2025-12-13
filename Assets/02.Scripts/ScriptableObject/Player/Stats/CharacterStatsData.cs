using Unity.Hierarchy;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Scriptable Object/Player/Stats Data")]
public class CharacterStatsData : ScriptableObject
{
    [Header("Stats")]
    public float maxHealth;


    [Header("Attack Stats")]
    public float attackDamage;
}
