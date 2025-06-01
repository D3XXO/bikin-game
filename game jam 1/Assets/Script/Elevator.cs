using UnityEngine;
using System.Collections.Generic;

public class Elevator : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float waypointThreshold = 0.1f;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Count == 0)
            return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < waypointThreshold)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= waypoints.Count)
        {
            currentWaypointIndex = 0;
        }
    }
}