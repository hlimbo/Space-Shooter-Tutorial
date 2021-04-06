using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 30f;
    [SerializeField]
    private float moveSpeed = 10f;

    private Animator animator;
    private SpawnManager spawnManager;

    private Coroutine delayDestructionRef;

    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // One way to rotate ~ calculates a brand new rotation to overwrite in the transform via localRotation property
        //Quaternion newRotation = transform.rotation * Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, Vector3.forward);
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, rotateSpeed * Time.deltaTime);
        
        // Another way to rotate but this applies the rotation on the transform internally
        transform.Rotate(0,0, rotateSpeed * Time.deltaTime, Space.Self);


        //transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Laser"))
        {
            if(delayDestructionRef == null)
            {
                animator.SetTrigger("onDestroyAsteroid");
                delayDestructionRef = StartCoroutine(DelayDestruction(other));
                audioSource.Play();
            }
        }
    }

    IEnumerator DelayDestruction(Collider2D other)
    {
        // Wait for one frame to ensure trigger is set to onDestroyAsteroid
        // so that the animator component can grab the explosion animation clip
        yield return null;
        var currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        spawnManager?.OnBeginSpawning(currentClipInfo[0].clip.length);
        Destroy(gameObject, currentClipInfo[0].clip.length);
        Destroy(other.gameObject);
    }
}
