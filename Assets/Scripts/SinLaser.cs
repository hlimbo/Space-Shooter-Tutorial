using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinLaser : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float thetaStep = Mathf.PI / 32f;
    [SerializeField]
    private float theta = 0f;
    [SerializeField]
    private float amplitude = 4f;

    private float xOffset;

    // how stretched or expanded the sine wave is
    // if number > 1, wave will shrink (meaning it will take a shorter time to reach a full sin wave cycle) 
    // if number < 1 but > 0,  wave will stretch out (meaning it will take longer to reach a full sine wave cycle)
    [SerializeField]
    private float waveFrequency = 2f;

    // Determines which direction the sine wave should go initially (e.g. left or right)
    [SerializeField]
    private int waveDirection = 1;

    void Start()
    {
        xOffset = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // go between 0 and 2pi
        // need a theta step every update
        // sin wave needs to move relative to the initial position it was shot from
        float newXPos = waveDirection * amplitude * Mathf.Sin(theta * waveFrequency) + xOffset;
        float xStep = newXPos - transform.position.x;

        transform.Translate(new Vector3(xStep, speed * Time.deltaTime));

        theta += thetaStep;

        // if out of bounds , destroy
        if(transform.position.y >= 7f)
        {
            Destroy(gameObject);
        }
    }
    
    public void Init(float ySpeed, float xOffset, int waveDirection = 1)
    {
        speed = ySpeed;
        this.xOffset = xOffset;
        this.waveDirection = waveDirection;
    }
}
