using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : BaseEnemyBrain
{
    private IShootable shootable;
    private IMovable movable;
    private Destructible destructible;

    [SerializeField]
    private float shotFrequency = 2f;
    private float lastShotTime = 0f;

    [SerializeField]
    private int maxShotCount = 2;
    private int currentShotCount = 0;

    [SerializeField]
    private bool enableShooting;

    private void Start()
    {
        lastShotTime = Time.time;
        movable = GetComponent<IMovable>();
        shootable = transform.GetComponentInChildren<IShootable>();
        destructible = GetComponent<Destructible>();
    }

    private void Update()
    {
        bool willBeDestroyed = destructible != null && destructible.WillBeDestroyed;

        if (!willBeDestroyed)
        {
            if (enableShooting)
            {
                if (Time.time - lastShotTime >= shotFrequency && currentShotCount < maxShotCount)
                {
                    shootable?.FireShot();
                    lastShotTime = Time.time;
                    ++currentShotCount;
                }
            }

            movable?.Move(Time.deltaTime);
        }
       
    }
}
