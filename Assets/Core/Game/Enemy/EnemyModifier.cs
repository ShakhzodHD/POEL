using UnityEngine;

[CreateAssetMenu(fileName = "EnemyModifier", menuName = "Enemies/Modifier")]
public class EnemyModifier : ScriptableObject
{
    public string modifierName;
    public float healthMultiplier = 1f;
    public float speedMultiplier = 1f;

    public void ApplyModifier(BaseEnemy enemy)
    {
        enemy.Health.CurrentHealth = Mathf.RoundToInt(enemy.Health.CurrentHealth * healthMultiplier);
        enemy.Speed *= speedMultiplier;
    }
}
