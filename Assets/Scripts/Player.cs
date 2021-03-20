// Part of .NET Framework
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello");
        // take current position and assign a new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    // Update is called 1 frame or 60 frames per second
    // 1 meter per frame => 60 meters per second approx
    // Want to move player 1 meter per second instead of 1 meter per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 horizontalTranslation = Vector3.right * horizontalInput * speed * Time.deltaTime;
        Vector3 verticalTranslation = Vector3.up * verticalInput * speed * Time.deltaTime;

        // completion in seconds for the last frame
        // transform.Translate(horizontalTranslation + verticalTranslation);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // Another way of doing this is treating horizontal and vertical inputs as a unit vector
        // or direction
        /*
         *
         * Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f);
         * transform.Translate(direction * speed * Time.deltaTime);
         *
         */

        // top and bottom bounds ~ Alternatively, we can use Mathf.Clamp to keep movement limited between a min and max y value
        if (transform.position.y <= -3.6f)
        {
            transform.position = new Vector3(transform.position.x, -3.6f, transform.position.z);
        }
        else if (transform.position.y >= 5.8f)
        {
            transform.position = new Vector3(transform.position.x, 5.8f, transform.position.z);
        }

        // left and right wrap-around
        if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, transform.position.z);
        }
    }
}
