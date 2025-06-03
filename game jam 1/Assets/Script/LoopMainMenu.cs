using UnityEngine;

public class LoopMainMenu : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    public float distance;
    
    [Header("Visual Settings")]
    public bool defaultFacingRight = true;
    
    private Vector3 startPosition;
    private bool isMovingRight;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        isMovingRight = defaultFacingRight;
        UpdateFacingDirection();
    }

    void Update()
    {
        float leftBound = startPosition.x - distance;
        float rightBound = startPosition.x + distance;
        
        if (isMovingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            
            if (transform.position.x >= rightBound)
            {
                isMovingRight = false;
                UpdateFacingDirection();
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            
            if (transform.position.x <= leftBound)
            {
                isMovingRight = true;
                UpdateFacingDirection();
            }
        }
    }

    void UpdateFacingDirection()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !isMovingRight;
        }
    }
}