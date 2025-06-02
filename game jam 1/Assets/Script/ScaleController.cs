using UnityEngine;
public class ScaleController : MonoBehaviour
{
    [Header("Scale Settings")]
    [Tooltip("Nilai positif untuk memperbesar, negatif untuk memperkecil")]
    public float scaleFactor;

    private Vector3 originalScale;
    private bool isMouseOver = false;
    private bool isScaled = false;
    void Start()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (isMouseOver)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (isScaled && transform.localScale != originalScale)
                {
                    ResetScale();
                }
                else
                {
                    ApplyScale(scaleFactor);
                    isScaled = true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (isScaled && transform.localScale != originalScale)
                {
                    ResetScale();
                }
                else
                {
                    ApplyScale(-scaleFactor);
                    isScaled = true;
                }
            }
        }
    }
    void ApplyScale(float factor)
    {
        if (factor > 0)
        {
            transform.localScale = originalScale * factor;
        }
        else if (factor < 0)
        {
            transform.localScale = originalScale / Mathf.Abs(factor);
        }
    }
    void ResetScale()
    {
        transform.localScale = originalScale;
        isScaled = false;
    }
    void OnMouseEnter()
    {
        isMouseOver = true;
    }
    void OnMouseExit()
    {
        isMouseOver = false;
    }
}