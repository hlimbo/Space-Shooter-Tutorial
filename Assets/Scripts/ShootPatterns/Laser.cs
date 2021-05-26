using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private int yDirection = 1;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * yDirection * Time.deltaTime);
    }
}
