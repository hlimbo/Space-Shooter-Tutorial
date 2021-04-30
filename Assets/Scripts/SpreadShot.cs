using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour
{
    [SerializeField]
    public Bullet bulletPrefab;
    [SerializeField]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    [ContextMenu("Fire Spread Shot Test")]
    private void FireSpreadShotTest()
    {
        Quaternion up = Quaternion.LookRotation(transform.forward, transform.up);

        var bullets = new Bullet[3];
        bullets[0] = Instantiate(bulletPrefab, transform.position, up, transform);
        bullets[1] = Instantiate(bulletPrefab, transform.position, up * Quaternion.Euler(0, 0, 45), transform);
        bullets[2] = Instantiate(bulletPrefab, transform.position, up * Quaternion.Euler(0, 0, -45), transform);

        foreach (var bullet in bullets)
        {
            bullet.Speed = speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
