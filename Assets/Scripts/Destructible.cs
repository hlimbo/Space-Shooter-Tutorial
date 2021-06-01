using System.Collections;
using UnityEngine;
 
public class Destructible : MonoBehaviour
{
    private Player player;
    private WaveManager waveManager;
    private SpawnManager spawnManager;

    private Animator animator;
    private Coroutine delayDestructionRef = null;
    private AudioSource audioSource;
    private new Collider2D collider2D;

    private IMovable[] movables;

    public bool WillBeDestroyed => delayDestructionRef != null;

    // Other 3d enemy asset if available
    private GameObject childObject;

    private Hurtable hurtable;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        waveManager = FindObjectOfType<WaveManager>();
        spawnManager = FindObjectOfType<SpawnManager>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        collider2D = GetComponent<Collider2D>();

        if(transform.childCount > 0)
        {
            childObject = transform?.GetChild(0)?.gameObject;
        }

        movables = GetComponentsInParent<IMovable>();
        hurtable = GetComponent<Hurtable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);

            if (hurtable == null || hurtable?.Hp <= 0)
            {
                if (delayDestructionRef == null)
                {
                    collider2D.enabled = false;
                    player?.AddScore(10);
                    animator?.SetTrigger("onEnemyDeath");
                    audioSource?.Play();
                    DisableMovement();
                    delayDestructionRef = StartCoroutine(DelayDestruction());

                    if (spawnManager != null)
                    {
                        // 40 percent chance an enemy drops a random power-up
                        if (Random.value <= 0.4f)
                        {
                            spawnManager.SpawnRandomPowerup(transform.position);
                        }
                    }
                }
            }
        }
        else if(other.tag.Equals("Player"))
        {
            if(delayDestructionRef == null)
            {
                player?.Damage();
                animator?.SetTrigger("onEnemyDeath");
                audioSource?.Play();
                DisableMovement();
                delayDestructionRef = StartCoroutine(DelayDestruction());
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

        if (childObject != null)
        {
            Destroy(childObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }
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
