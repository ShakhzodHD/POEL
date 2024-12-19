using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;

    [SerializeField] private float activationRadius = 10f;
    [SerializeField] private float deactivationRadius = 20f;

    private Transform player;
    public void Initialize(Transform player)
    {
        this.player = player;
    }

    private void Update()
    {
        foreach (var enemyObject in enemies)
        {
            if (enemyObject == null) continue;

            float distance = Vector3.Distance(player.position, enemyObject.transform.position);

            if (distance <= activationRadius && !enemyObject.activeSelf)
            {
                ActivateEnemy(enemyObject);
            }
            else if (distance > deactivationRadius && enemyObject.activeSelf)
            {
                DeactivateEnemy(enemyObject);
            }
        }
    }

    private void ActivateEnemy(GameObject enemy)
    {
        if (enemy.TryGetComponent<BaseEnemyBehavior>(out var enemyBehavior))
        {
            enemyBehavior.Initialize(player);
            enemy.SetActive(true);
        }
    }

    private void DeactivateEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, activationRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, deactivationRadius);
    }
}
