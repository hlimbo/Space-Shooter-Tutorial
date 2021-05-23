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

    private IMovable[] movables;

    [SerializeField]
    private int hp = 1;

    public bool WillBeDestroyed => delayDestructionRef != null;

    // Other 3d enemy asset if available
    private GameObject childObject;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        waveManager = FindObjectOfType<WaveManager>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if(transform.childCount > 0)
        {
            childObject = transform?.GetChild(0)?.gameObject;
        }

        movables = GetComponentsInParent<IMovable>();
    }

    // TODO add special effect
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            --hp;
            Destroy(other.gameObject);
            if (hp <= 0)
            {
                if (delayDestructionRef == null)
                {
                    player?.AddScore(10);
                    animator.SetTrigger("onEnemyDeath");
                    audioSource.Play();
                    DisableMovement();
                    delayDestructionRef = StartCoroutine(DelayDestruction());

                    if (childObject != null)
                    {
                        Destroy(childObject);
                    }
                }
            }
        }
        else if(other.tag.Equals("Player"))
        {
            hp = 0;
            if(delayDestructionRef == null)
            {
                player?.Damage();
                animator.SetTrigger("onEnemyDeath");
                audioSource.Play();
                DisableMovement();
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
            Debug.Log("Decrementing Enemy Count: " + name);
            waveManager?.DecrementEnemyCount();
        }
    }

    IEnumerator DelayDestruction ()
    {
        yield return null;
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    private void DisableMovement ()
    {
        if (movables != null && movables.Length > 0)
        {
            foreach(var movable in movables)
            {
                (movable as MonoBehaviour).enabled = false;
            }
        }
    }
}
