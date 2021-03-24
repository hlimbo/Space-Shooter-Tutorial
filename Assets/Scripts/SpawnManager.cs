using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPrefab;
    [SerializeField]
    private float spawnDelay = 5f;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private void OnDestroy()
    {
        Debug.Log("Killing coroutines");
        StopAllCoroutines();
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(!stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(RandomXPosition(), 6.5f, 0f);
            GameObject enemy = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.SetParent(enemyContainer.transform);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while(!stopSpawning)
        {
            int randomDelay = Random.Range(3, 7);
            yield return new WaitForSeconds(randomDelay);
            Vector3 spawnPosition = new Vector3(RandomXPosition(), 6.5f, 0f);
            var randomPowerUp = powerups[(int)PowerUp.Extensions.GetRandomPowerUp()];
            Instantiate(randomPowerUp, spawnPosition, Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }

    private float RandomXPosition()
    {
        return Random.Range(-9f, 9f);
    }
}
