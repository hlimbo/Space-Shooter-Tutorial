using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMover : MonoBehaviour, IMovable
{
    [SerializeField]
    private float moveBackDelayInSeconds;

    [SerializeField]
    private float maxTravelDistance;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    private float speed;

    private Camera mainCamera;
    private float currentDistanceTraveled = 0f;
    private float startTime = -1f;

    void Awake()
    {
        mainCamera = FindObjectOfType<Camera>();

        // Need to set only for big enemy player only
        //transform.position = new Vector3(
        //    mainCamera.transform.position.x,
        //    mainCamera.transform.position.y + mainCamera.orthographicSize + yOffset,
        //    0f
        //);
    }

    public void Move(float deltaTime)
    {
        if(currentDistanceTraveled < maxTravelDistance)
        {
            Vector3 deltaDistance = transform.up * speed * deltaTime;
            currentDistanceTraveled += deltaDistance.magnitude;
            transform.Translate(deltaDistance, Space.World);
        }
        else if(currentDistanceTraveled >= maxTravelDistance && startTime == -1f)
        {
            startTime = Time.time;
        }
        else if(Time.time - startTime >= moveBackDelayInSeconds)
        {
            transform.Translate(-transform.up * speed * deltaTime, Space.World);
        }
    }
}
