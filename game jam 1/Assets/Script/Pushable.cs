using UnityEngine;

public class Pushable : MonoBehaviour
{
    [Header("Scale Settings")]
    public float scaleFactor;
    public float pushableScaleThreshold;

    [Header("Physics Settings")]
    public float normalMass;
    public float heavyMass;
    [Tooltip("Prevents objects from becoming too light when small")]
    public float minMass;

    private Vector3 originalScale;
    private bool isMouseOver = false;
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

            if (Input.GetMouseButtonDown(0))
            {
                MakeSmaller();
            }
        }
    }

    void MakeSmaller()
    {
        transform.localScale = originalScale / scaleFactor;
        UpdatePushability();
    }

    void UpdatePushability()
    {
        float currentScale = transform.localScale.x;
        float scaleRatio = currentScale / originalScale.x;

        if (scaleRatio <= pushableScaleThreshold)
        {
            rb.isKinematic = false;
            rb.mass = Mathf.Max(normalMass * scaleRatio, minMass);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.tag = "Pushable";
        }
        else
        {
            rb.isKinematic = false;
            rb.mass = heavyMass;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.tag = "Ground";
        }

        if (col != null)
        {
            col.enabled = true;
        }

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
