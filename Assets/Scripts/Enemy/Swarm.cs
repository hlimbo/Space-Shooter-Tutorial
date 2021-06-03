using SplineFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    [SerializeField]
    private int swarmCount;
    public int SwarmCount => swarmCount;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnDelay = 1f;
    public float SpawnDelay => spawnDelay;

    private BezierSpline path;

    private void Awake()
    {
        path = GetComponent<BezierSpline>();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine ()
    {
        if (path != null)
        {
            for(int i = 0;i < swarmCount; ++i)
            {
                var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
                enemy.GetComponent<PathFollower>().AssignPath(path);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        yield return DestroySwarmContainer();
    }

    private IEnumerator DestroySwarmContainer ()
    {
        while (transform.childCount > 0)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
