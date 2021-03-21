using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPrefab;
    [SerializeField]
    private float spawnDelay = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        Debug.Log("Killing coroutine");
        // Kill Coroutine
        StopCoroutine(SpawnRoutine());
    }

    // Challenge: Spawn game objects every 5 seconds
    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Vector3 spawnPosition = new Vector3(0f, 6.5f, 0f);
            Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
