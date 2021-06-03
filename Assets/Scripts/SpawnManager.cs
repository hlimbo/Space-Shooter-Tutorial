using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private float spawnDelay = 10f;

    private bool stopSpawning = false;

    private Camera mainCamera;
    [SerializeField]
    private float powerupYOffset = 2f;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        // Toggle this script on/off
    }

    public void OnBeginSpawning(float secondsDelay)
    {
        // Stop All coroutines to prevent the same routine running more than once
        StopAllCoroutines();
        StartCoroutine(BeginSpawningRoutine(secondsDelay));
    }

    IEnumerator BeginSpawningRoutine(float secondsDelay)
    {
        yield return new WaitForSeconds(secondsDelay);
        StartCoroutine(SpawnPowerupRoutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    IEnumerator SpawnPowerupRoutine()
    {
        while(!stopSpawning)
        {
            float spawnHeight = mainCamera.transform.position.y + mainCamera.orthographicSize + powerupYOffset;
            yield return new WaitForSeconds(spawnDelay);
            Vector3 spawnPosition = new Vector3(RandomXPosition(), spawnHeight, 0f);
            // 90 percent chance a power-up will spawn from the sky
            //if (Random.value <= 0.9f)
            //{
                SpawnRandomPowerup(spawnPosition);
            //}
            
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }

    private float RandomXPosition()
    {
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float minX = mainCamera.transform.position.x - (camWidth / 2f);
        float maxX = mainCamera.transform.position.x + (camWidth / 2f);
        return Random.Range(minX, maxX);
    }

    public void SpawnRandomPowerup (Vector3 spawnPosition)
    {
        var randomPowerUp = powerups[(int)PowerUp.Extensions.GetWeightedRandomPowerUp()];
        Instantiate(randomPowerUp, spawnPosition, Quaternion.identity);
    }
}
