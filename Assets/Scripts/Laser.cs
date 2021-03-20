using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Move Laser upwards
    // Speed Variable - move 8 meters per second
    [SerializeField]
    private float speed = 8f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        // if laser is offscreen or above 7 units from the origin
        if(transform.position.y > 7f)
        {
            // delete the bullet
            Destroy(gameObject);
        }
    }
}
