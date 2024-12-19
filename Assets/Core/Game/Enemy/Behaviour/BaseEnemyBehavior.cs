using UnityEngine;

public class BaseEnemyBehavior : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    public virtual void Initialize(Transform player)
    {
        enemyAI.SetPlayerTransform(player);
    }
}
