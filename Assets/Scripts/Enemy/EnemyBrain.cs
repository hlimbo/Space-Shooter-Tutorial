using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private IShootable shootable;
    private IMovable movable;

    [SerializeField]
    private float shotFrequency = 2f;
    private float lastShotTime = 0f;

    [SerializeField]
    private int maxShotCount = 2;
    private int currentShotCount = 0;

    private void Start()
    {
        lastShotTime = Time.time;
        movable = GetComponent<IMovable>();
        shootable = transform.GetComponentInChildren<IShootable>();
    }

    private void Update()
    {
        if (Time.time - lastShotTime >= shotFrequency && currentShotCount < maxShotCount)
        {
            shootable?.FireShot();
            lastShotTime = Time.time;
            ++currentShotCount;
        }

        movable?.Move(Time.deltaTime);
    }
}
