using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    private HomingMover mover;
    private BaseEnemyBrain[] enemies;

    private void Start()
    {
        mover = GetComponent<HomingMover>();
        enemies = FindObjectsOfType<BaseEnemyBrain>();

        SelectClosestTarget(); 
    }

    private void Update()
    {
        mover?.Move(Time.deltaTime);
    }

    private void SelectClosestTarget ()
    {
        if (enemies.Length > 0)
        {
            var closestEnemy = enemies[0];
            float closestDistance = (closestEnemy.transform.position - transform.position).magnitude;

            for (int i = 1; i < enemies.Length; ++i)
            {
                var enemy = enemies[i];
                float distance = (enemy.transform.position - transform.position).magnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            mover.SetTarget(closestEnemy.gameObject);
        }
    }
}
