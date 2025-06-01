using UnityEngine;

public class BoxScaleController : MonoBehaviour
{
    [Header("Scale Settings")]
    public float scaleFactor = 2f;
    public float pushableScaleThreshold = 1.2f;

    [Header("Physics Settings")]
    public float normalMass = 1f;
    public float heavyMass = 100f;
    [Tooltip("Prevents objects from becoming too light when small")]
    public float minMass = 0.2f;

    private Vector3 originalScale;
    private bool isMouseOver = false;
    private bool isScaled = false;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.freezeRotation = true;
        UpdatePushability();
    }

    void Update()
    {
        if (isMouseOver)
        {
            if (Input.GetMouseButtonDown(1)) // Right click - make bigger
            {
                if (isScaled)
                {
                    ResetScale();
                }
                else
                {
                    MakeBigger();
                }
            }

            if (Input.GetMouseButtonDown(0)) // Left click - make smaller
            {
                if (isScaled)
                {
                    ResetScale();
                }
                else
                {
                    MakeSmaller();
                }
            }
        }
    }

    void MakeBigger()
    {
        transform.localScale = originalScale * scaleFactor;
        isScaled = true;
        UpdatePushability();
    }

    void MakeSmaller()
    {
        transform.localScale = originalScale / scaleFactor;
        isScaled = true;
        UpdatePushability();
    }

    void ResetScale()
    {
        transform.localScale = originalScale;
        isScaled = false;
        UpdatePushability();
    }

    void UpdatePushability()
    {
        float currentScale = transform.localScale.x; // Using X axis as reference
        float scaleRatio = currentScale / originalScale.x;

        if (scaleRatio <= pushableScaleThreshold)
        {
            // Pushable state
            rb.isKinematic = false;
            rb.mass = Mathf.Max(normalMass * scaleRatio, minMass); // Never goes below minMass
            gameObject.tag = "Pushable";

            // Ensure collider is active
            if (col != null)
            {
                col.enabled = true;
            }
        }
        else
        {
            // Immovable state
            rb.isKinematic = true;
            rb.mass = heavyMass;
            gameObject.tag = "Immovable";
        }

        // Force physics update
        Physics2D.SyncTransforms();
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }

    public bool CanBePushed()
    {
        return !rb.isKinematic;
    }
}