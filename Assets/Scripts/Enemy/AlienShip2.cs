using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// May rename later to be how the ship moves CMovement
public class AlienShip2 : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private float angle = 0f;
    [SerializeField]
    private float angleStep = Mathf.PI / 32f;
    [SerializeField]
    private int angleRotation = -1; // clockwise / + counter clockwise

    [SerializeField]
    private Transform centerRotation;

    [SerializeField]
    private float startAngle = Mathf.PI / 2;
    [SerializeField]
    private float endAngle = -Mathf.PI / 2;

    // Start is called before the first frame update
    void Start()
    {
        angle = startAngle;
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        // need to determine if moving in negative or positive direction
        if(angle > endAngle)
        {
            UpdatePosition();
            angle += angleStep * Time.deltaTime * angleRotation;
        }
    }

    private void UpdatePosition ()
    {
        float x = Mathf.Cos(angle) * radius + centerRotation.position.x;
        float y = Mathf.Sin(angle) * radius + centerRotation.position.y;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
