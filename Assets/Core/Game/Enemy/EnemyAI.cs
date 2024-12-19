using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Transform player;

    public float detectionRange = 10f; // ������ ����������� ������
    public float attackRange = 2f; // ��������� �����
    public float movementSpeed = 3f; // �������� ��������
    public float attackCooldown = 1.5f; // ������� �����

    private bool isAggroed = false;
    private bool isAttacking = false;
    private Vector3 startPosition;
    private float nextAttackTime;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAggroed)
        {
            isAggroed = true;
        }

        if (isAggroed)
        {
            if (distanceToPlayer > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                if (Time.time >= nextAttackTime)
                {
                    StartCoroutine(AttackPlayer());
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        float patrolRadius = 3f;
        Vector3 patrolPoint = startPosition + new Vector3(Mathf.Sin(Time.time) * patrolRadius, 0, Mathf.Cos(Time.time) * patrolRadius);
        transform.position = Vector3.MoveTowards(transform.position, patrolPoint, movementSpeed * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed * Time.deltaTime);
        transform.LookAt(player);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        // �������� ����� (���� ����)
        Debug.Log($"{name} ������� ������!");

        // �������� ��� ����� (��������)
        yield return new WaitForSeconds(0.5f);

        // ��������� ����� (������� �� ������� �������� ������)
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log($"{name} ������� ����!");
            // player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }

        isAttacking = false;
    }
    public void SetPlayerTransform(Transform player)
    {
        this.player = player;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
