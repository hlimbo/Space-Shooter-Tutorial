using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyBrain : MonoBehaviour
{
    private IMovable movable;
    private IShootable shootable;
    private Destructible destructible;

    private Player player;

    [SerializeField]
    private int shotCount = 3;

    [SerializeField]
    private float yShotOffset = 4f;

    private Coroutine shootRoutine;

    void Awake()
    {
        movable = GetComponent<IMovable>();
        shootable = GetComponentInChildren<IShootable>();
        destructible = GetComponent<Destructible>();

        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        bool willBeDestroyed = destructible != null && destructible.WillBeDestroyed;

        if (!willBeDestroyed)
        {
            movable?.Move(Time.deltaTime);

            if (player != null && shotCount > 0)
            {
                Vector3 diff = transform.position - player.transform.position;
                // below player shoot
                if (shootRoutine == null && diff.y + yShotOffset < 0f)
                {
                    shootRoutine = StartCoroutine(ShootRoutine());
                }

            }
        }
    }

    IEnumerator ShootRoutine()
    {
        while (shotCount > 0)
        {
            shootable?.FireShot();
            --shotCount;
            yield return new WaitForSeconds(1f);
        }
    }
}
