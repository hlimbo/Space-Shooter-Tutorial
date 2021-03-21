using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Medium Article: Creating an Enemy

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move 4 meters per second
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // if bottom of screen -> respawn at top at a random x position
        if(transform.position.y < -4.5f)
        {
            float randomXPosition = UnityEngine.Random.Range(-9f, 9f);
            transform.position = new Vector3(randomXPosition, 5, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if(other.tag.Equals("Player"))
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
                Destroy(gameObject);
            }
        }
    }
}
