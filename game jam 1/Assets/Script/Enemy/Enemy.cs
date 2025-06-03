using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float followSpeed;
    [SerializeField] private float recordInterval;
    [SerializeField] private float closeRadius;
    [SerializeField] private float positionTolerance;
    
    [Header("Vision Settings")]
    [SerializeField] private float visionDistance;
    [SerializeField] private float visionAngle;
    [SerializeField] private LayerMask obstacleLayers;

    public Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private List<Vector2> playerPositions = new List<Vector2>();
    private float lastRecordTime;
    private bool isMovementPaused = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        bool canSeePlayer = CanSeePlayer();
        
        if (canSeePlayer && Time.time - lastRecordTime > recordInterval && !isMovementPaused)
        {
            playerPositions.Add(player.position);
            lastRecordTime = Time.time;
        }

        CheckPauseMovement();

        if (!isMovementPaused)
        {
            if (Vector2.Distance(transform.position, player.position) <= closeRadius && canSeePlayer)
            {
                FollowPlayerDirectly();
            }
            else if (playerPositions.Count > 0)
            {
                FollowRecordedPath();
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer > visionDistance) return false;
        
        float angleToPlayer = Vector2.Angle(GetFacingDirection(), directionToPlayer);
        if (angleToPlayer > visionAngle / 2f) return false;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayers);
        if (hit.collider != null && !hit.collider.CompareTag("Player")) return false;
        
        return true;
    }

    Vector2 GetFacingDirection()
    {
        if (spriteRenderer == null)
            return Vector2.right;

        return spriteRenderer.flipX ? Vector2.right : Vector2.left;
    }

    void FollowPlayerDirectly()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * followSpeed, rb.velocity.y);
        FlipSprite(direction);
    }

    void FollowRecordedPath()
    {
        if (playerPositions.Count == 0) return;

        Vector2 targetPos = playerPositions[0];
        float direction = Mathf.Sign(targetPos.x - transform.position.x);
        rb.velocity = new Vector2(direction * followSpeed, rb.velocity.y);
        FlipSprite(direction);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            playerPositions.RemoveAt(0);
        }
    }

    void FlipSprite(float direction)
    {
        spriteRenderer.flipX = direction > 0;
    }

    void CheckPauseMovement()
    {
        float xDifference = Mathf.Abs(player.position.x - transform.position.x);

        if (xDifference <= positionTolerance)
        {
            isMovementPaused = true;
        }
        else if (isMovementPaused && xDifference > positionTolerance)
        {
            isMovementPaused = false;
            playerPositions.Clear();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float halfAngle = visionAngle / 2f;
        Vector2 facingDirection = GetFacingDirection();
        
        Vector2 leftDir = Quaternion.Euler(0, 0, halfAngle) * facingDirection;
        Vector2 rightDir = Quaternion.Euler(0, 0, -halfAngle) * facingDirection;
        
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + leftDir * visionDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + rightDir * visionDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeRadius);
    }
}