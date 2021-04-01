using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;

    private Player player;
    private Animator animator;
    private Coroutine delayDestructionRef;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        if(player == null)
        {
            Debug.LogError("Player reference missing on Enemy.cs script");
        }

        animator = GetComponent<Animator>();
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
            if(delayDestructionRef == null)
            {
                Destroy(other.gameObject);
                player?.AddScore(10);
                animator.SetTrigger("onEnemyDeath");
                speed = 0;
                delayDestructionRef = StartCoroutine(DelayDestruction());
            }
        }
        else if(other.tag.Equals("Player"))
        {
            if(delayDestructionRef == null)
            {
                player?.Damage();
                animator.SetTrigger("onEnemyDeath");
                speed = 0;
                delayDestructionRef = StartCoroutine(DelayDestruction());
            }
        }
    }

    IEnumerator DelayDestruction ()
    {
        // Perhaps create a Unity article about this
        yield return null;
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }
}
