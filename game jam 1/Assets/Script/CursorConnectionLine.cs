using UnityEngine;

public class CursorConnectionLine : MonoBehaviour
{
    [Header("Line Settings")]
    public float lineWidth;
    public Material lineMaterial;

    [Tooltip("Layer order for the line")]
    public int sortingOrder;
    
    private LineRenderer lineRenderer;
    private Transform playerTransform;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
        lineRenderer.sortingOrder = sortingOrder;
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        
        lineRenderer.SetPosition(0, playerTransform.position);
        lineRenderer.SetPosition(1, mousePosition);
    }
}