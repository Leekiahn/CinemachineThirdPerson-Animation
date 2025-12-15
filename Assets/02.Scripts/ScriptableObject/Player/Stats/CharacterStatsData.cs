using Unity.Hierarchy;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Scriptable Object/Player/Stats Data")]
public class CharacterStatsData : ScriptableObject
{
    [Header("Stats")]
    [field: SerializeField] public float maxHealth { get; private set; }


    [Header("Attack Stats")]
    public float attackDamage;
}
