using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownMover : MonoBehaviour, IMovable
{
    [SerializeField]
    private float speed = 4f;

    public void Move(float deltaTime)
    {
        transform.Translate(Vector3.down * speed * deltaTime);
    }

    private void Update()
    {
        Move(Time.deltaTime);
    }
}
