using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroEnemyBrain : BaseEnemyBrain
{
    private IMovable mainMovable;
    private IMovable secondaryMovable;
    private Destructible destructible;

    private Player player;

    [SerializeField]
    private float triggerDistance = 5f;

    private void Start()
    {
        mainMovable = GetComponent<DownMover>();
        secondaryMovable = GetComponent<HomingMover>();
        destructible = GetComponent<Destructible>();

        player = FindObjectOfType<Player>();

        StartCoroutine(CheckPlayerProximity());
    }

    private void Update()
    {
        bool willBeDestroyed = destructible != null && destructible.WillBeDestroyed;
        if (!willBeDestroyed)
        {
            mainMovable?.Move(Time.deltaTime);
            secondaryMovable?.Move(Time.deltaTime);
        }
    }

    private IEnumerator CheckPlayerProximity ()
    {
        // Detect if we can switch movement patterns based on the distance between this enemy and the player
        float enemyPlayerDistance = (transform.position - player.transform.position).magnitude;
        while(enemyPlayerDistance > triggerDistance)
        {
            yield return null;
            enemyPlayerDistance = (transform.position - player.transform.position).magnitude;
        }

        (mainMovable as MonoBehaviour).enabled = false;

        var homingMover = (HomingMover)secondaryMovable;
        homingMover.enabled = true;
        homingMover.SetTarget(player.gameObject);
    }
}
