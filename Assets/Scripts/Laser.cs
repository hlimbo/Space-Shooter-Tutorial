using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Move Laser upwards
    // Speed Variable - move 8 meters per second
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private int yDirection = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * yDirection * Time.deltaTime);
        
        if(transform.position.y > 7f || transform.position.y < -7f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
