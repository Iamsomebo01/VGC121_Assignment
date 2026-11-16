
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform pointA;
    public Transform pointB;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Transform player;

    private int direction = 1;
    private float attackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Find the player
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;

            // Ignore collision between enemy and player
            Collider2D playerCollider =
                playerObject.GetComponent<Collider2D>();

            Collider2D enemyCollider =
                GetComponent<Collider2D>();

            if (playerCollider != null && enemyCollider != null)
            {
                Physics2D.IgnoreCollision(
                    enemyCollider,
                    playerCollider,
                    true
                );
            }
        }
        else
        {
            Debug.LogWarning("Player not found! Check Player tag.");
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(
                rb.position,
                player.position
            );

            if (distance <= attackRange)
            {
                Attack();
                return;
            }
        }

        Patrol();
    }

    void Patrol()
    {
        if (pointA == null || pointB == null)
            return;

        float leftLimit = Mathf.Min(pointA.position.x, pointB.position.x);
        float rightLimit = Mathf.Max(pointA.position.x, pointB.position.x);

        float nextX = rb.position.x +
            direction * moveSpeed * Time.fixedDeltaTime;

        if (nextX >= rightLimit)
        {
            direction = -1;
            nextX = rightLimit;
        }
        else if (nextX <= leftLimit)
        {
            direction = 1;
            nextX = leftLimit;
        }

        rb.MovePosition(new Vector2(nextX, rb.position.y));

        if (animator != null)
            animator.SetBool("IsWalking", true);

        if (spriteRenderer != null)
            spriteRenderer.flipX = direction < 0;
    }

    void Attack()
    {
        if (animator != null)
            animator.SetBool("IsWalking", false);

        if (spriteRenderer != null)
            spriteRenderer.flipX = player.position.x < rb.position.x;

        attackTimer -= Time.fixedDeltaTime;

        if (attackTimer <= 0)
        {
            Debug.Log("Enemy is attacking!");

            if (animator != null)
                animator.SetTrigger("attack");

            attackTimer = attackCooldown;
        }
    }
}
