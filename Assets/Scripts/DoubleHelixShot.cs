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

    // 2 sin laser behaviours
    // 4 shots with 0.25f seconds delay in between spawning

    private SinLaser lastLeftHelixPiece;
    private SinLaser lastRightHelixPiece;

    void Start()
    {
        xOffset = transform.position.x;
        StartCoroutine(SpawnHelixRoutine());
    }

    void Update ()
    {
        //if(lastLeftHelixPiece != null && lastRightHelixPiece != null)
        //{
        //    if (lastRightHelixPiece.transform.position.y >= 7f && lastRightHelixPiece.transform.position.y >= 7f)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
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

            if(pairCount == projectilePairCount)
            {
                lastLeftHelixPiece = leftHelix;
                lastRightHelixPiece = rightHelix;
            }
        }

        Destroy(gameObject);
    }
}
