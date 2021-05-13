using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    private Player player;
    private WaveManager waveManager;

    private Animator animator;
    private Coroutine delayDestructionRef = null;
    private AudioSource audioSource;

    public bool WillBeDestroyed => delayDestructionRef != null;

    // Other 3d enemy asset if available
    private GameObject childObject;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        waveManager = FindObjectOfType<WaveManager>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        childObject = transform?.GetChild(0)?.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            if(delayDestructionRef == null)
            {
                player?.AddScore(10);
                animator.SetTrigger("onEnemyDeath");
                audioSource.Play();
                delayDestructionRef = StartCoroutine(DelayDestruction());

                Destroy(other.gameObject);

                if(childObject != null)
                {
                    Destroy(childObject);
                }
            }
        }
        else if(other.tag.Equals("Player"))
        {
            if(delayDestructionRef == null)
            {
                player?.Damage();
                animator.SetTrigger("onEnemyDeath");
                audioSource.Play();
                delayDestructionRef = StartCoroutine(DelayDestruction());

                if (childObject != null)
                {
                    Destroy(childObject);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(tag.Equals("Enemy"))
        {
            waveManager?.DecrementEnemyCount();
        }
    }

    IEnumerator DelayDestruction ()
    {
        yield return null;
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }
}
