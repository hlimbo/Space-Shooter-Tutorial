using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHelixShot : MonoBehaviour
{
    [SerializeField]
    private float ySpeed;
    private float xOffset;

    [SerializeField]
    private SinLaser sinLaserPrefab;
    [SerializeField]
    private int projectilePairCount = 6;

    void Start()
    {
        xOffset = transform.position.x;
        StartCoroutine(SpawnHelixRoutine());
    }

    IEnumerator SpawnHelixRoutine()
    {
        int pairCount = 0;
        while(pairCount < projectilePairCount)
        {
            ++pairCount;
            var leftHelix = Instantiate(sinLaserPrefab, transform.position, Quaternion.identity);
            leftHelix.Init(ySpeed, xOffset, -1);
            var rightHelix = Instantiate(sinLaserPrefab, transform.position, Quaternion.identity);
            rightHelix.Init(ySpeed, xOffset);

            yield return new WaitForSeconds(0.06f);
        }

        Destroy(gameObject);
    }
}
