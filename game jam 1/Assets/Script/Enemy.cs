using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private float recordInterval;
    [SerializeField] private float closeRadius;
    [SerializeField] private float positionTolerance;

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
        if (Time.time - lastRecordTime > recordInterval && !isMovementPaused)
        {
            playerPositions.Add(player.position);
            lastRecordTime = Time.time;
        }

        CheckPauseMovement();

        if (!isMovementPaused)
        {
            if (Vector2.Distance(transform.position, player.position) <= closeRadius)
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
        if (direction > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction < 0)
        {
            spriteRenderer.flipX = false;
        }
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
}
