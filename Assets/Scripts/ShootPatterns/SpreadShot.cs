using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour, IShootable
{
    [SerializeField]
    public Bullet bulletPrefab;
    [SerializeField]
    public float speed;

    [ContextMenu("Fire Spread Shot Test")]
    private void FireSpreadShot()
    {
        Quaternion up = Quaternion.LookRotation(transform.forward, transform.up);

        var bullets = new Bullet[3];
        bullets[0] = Instantiate(bulletPrefab, transform.position, up, null);
        bullets[1] = Instantiate(bulletPrefab, transform.position, up * Quaternion.Euler(0, 0, 45), null);
        bullets[2] = Instantiate(bulletPrefab, transform.position, up * Quaternion.Euler(0, 0, -45), null);

        foreach (var bullet in bullets)
        {
            bullet.Speed = speed;
        }
    }

    void IShootable.FireShot()
    {
        FireSpreadShot();
    }
}
