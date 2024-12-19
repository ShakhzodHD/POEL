using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Transform player;

    public float detectionRange = 10f; // Радиус обнаружения игрока
    public float attackRange = 2f; // Дистанция атаки
    public float movementSpeed = 3f; // Скорость движения
    public float attackCooldown = 1.5f; // Кулдаун атаки

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

        // Анимация атаки (если есть)
        Debug.Log($"{name} атакует игрока!");

        // Задержка для атаки (анимация)
        yield return new WaitForSeconds(0.5f);

        // Нанесение урона (зависит от системы здоровья игрока)
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Debug.Log($"{name} наносит урон!");
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
