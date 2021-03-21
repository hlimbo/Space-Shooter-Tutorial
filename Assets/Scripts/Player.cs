// Part of .NET Framework
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private float spawnOffset = 0.8f;
    [SerializeField]
    private float fireDelay = 0.5f;
    // Seconds until new laser shot can be fired
    private float newFireTime = -1f;
    [SerializeField]
    private int lives = 3;

    // Start is called before the first frame update
    void Start()
    {
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
        
        if (Input.GetKeyDown(KeyCode.Space) && newFireTime < Time.time)
        {
            FireLaser();
        }
    }

    void CalculateMovement ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

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

    void FireLaser()
    {
        newFireTime = Time.time + fireDelay;
        Vector3 laserOffset = Vector3.up * spawnOffset;
        Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
    }

    public void Damage()
    {
        --lives;
        // Delete Player if no more lives left
        if (lives < 1)
        {
            Destroy(gameObject);
        }
    }
}
