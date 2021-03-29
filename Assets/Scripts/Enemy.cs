using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            player?.AddScore(10);
            Destroy(gameObject);
        }
        else if(other.tag.Equals("Player"))
        {
            player?.Damage();
            Destroy(gameObject);
        }
    }
}
