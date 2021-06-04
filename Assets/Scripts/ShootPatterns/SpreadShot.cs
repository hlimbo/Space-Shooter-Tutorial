using UnityEngine;

public class SpreadShot : MonoBehaviour, IShootable
{
    [SerializeField]
    private Laser laserPrefab;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int bulletsPerShot = 3;

    [ContextMenu("Fire Spread Shot Test")]
    private void FireSpreadShot()
    {
        Quaternion up = Quaternion.LookRotation(transform.forward, transform.up);

        var lasers = new Laser[bulletsPerShot];
        lasers[0] = Instantiate(laserPrefab, transform.position, up, null);
        lasers[1] = Instantiate(laserPrefab, transform.position, up * Quaternion.Euler(0, 0, 45), null);
        lasers[2] = Instantiate(laserPrefab, transform.position, up * Quaternion.Euler(0, 0, -45), null);

        foreach (var laser in lasers)
        {
            laser.Speed = speed;
        }
    }

    void IShootable.FireShot()
    {
        FireSpreadShot();
    }
}
