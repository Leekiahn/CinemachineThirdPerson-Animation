using Unity.Hierarchy;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Scriptable Object/Player/Stats Data")]
public class PlayerStatsData : ScriptableObject
{
    [Header("Attack Stats")]
    public float attackDamage;
}
