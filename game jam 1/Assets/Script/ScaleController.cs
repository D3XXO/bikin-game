using UnityEngine;

public class ScaleController : MonoBehaviour
{
    [Header("Scale Settings")]
    [Tooltip("Faktor untuk memperbesar object")]
    public float scaleUpFactor;
    
    [Tooltip("Faktor untuk memperkecil object")]
    public float scaleDownFactor;

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
                    ApplyScale(scaleUpFactor);
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
                    ApplyScale(1/scaleDownFactor);
                    isScaled = true;
                }
            }
        }
    }

    void ApplyScale(float factor)
    {
        transform.localScale = originalScale * factor;
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