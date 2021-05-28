using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMover : MonoBehaviour, IMovable
{
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float speed;
    private float tolerance = 0.1f;

    private Transform targetWaypoint;
    private int waypointIndex = 0;

    void Start ()
    {
        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[waypointIndex];
        }
    }

    // This function is added to allow this script to be enabled/disabled
    private void Update()
    {

    }

    public void Move(float deltaTime)
    {
        if (enabled)
        {
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            transform.Translate(direction * speed * deltaTime, Space.World);

            float distance = (targetWaypoint.position - transform.position).magnitude;
            if (distance <= tolerance)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[waypointIndex];
            }
        }
    }
}
