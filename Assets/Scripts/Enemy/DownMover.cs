using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMover : MonoBehaviour, IMovable
{
    [SerializeField]
    private float speed = 4f;

    public void Move(float deltaTime)
    {
        if(enabled)
        {
            transform.Translate(transform.up * speed * deltaTime, Space.World);
        }
    }

    // This function is added to allow this script to be enabled/disabled
    private void Update()
    {
        
    }
}
