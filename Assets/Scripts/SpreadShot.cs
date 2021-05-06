using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour
{
    [SerializeField]
    public Bullet bulletPrefab;
    [SerializeField]
    public float speed;

    // Move this logic perhaps into another script that is composed of
    // Shooting Pattern Component
    // Movement Pattern Component
    // Can be called enemy brain
    [SerializeField]
    private float shotFrequency = 2f;
    private float lastShotTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lastShotTime = Time.time;
    }

    [ContextMenu("Fire Spread Shot Test")]
    private void FireSpreadShot()
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
        if(Time.time - lastShotTime >= shotFrequency)
        {
            FireSpreadShot();
            lastShotTime = Time.time;
        }
    }
}
