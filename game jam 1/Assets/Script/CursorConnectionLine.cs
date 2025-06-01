using UnityEngine;

public class CursorConnectionLine : MonoBehaviour
{
    [Header("Line Settings")]
    public float lineWidth = 0.1f;
    public Material lineMaterial;
    public Color lineColor = Color.white;
    
    private LineRenderer lineRenderer;
    private Transform playerTransform;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            lineRenderer.enabled = true;
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            
            lineRenderer.SetPosition(0, playerTransform.position);
            lineRenderer.SetPosition(1, mousePosition);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}