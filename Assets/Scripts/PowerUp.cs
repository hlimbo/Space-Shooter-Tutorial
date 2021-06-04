using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        TRIPLE_SHOT,
        SPEED_BOOST,
        SHIELD,
        AMMO,
        HEALTH,
        DOUBLE_HELIX,
        HOMING_LASER,
    };

    private static PowerUpType[] powerUpTypes = Enum.GetValues(typeof(PowerUpType))
        .Cast<PowerUpType>().ToArray();

    private static Dictionary<PowerUpType, int> weightTable = new Dictionary<PowerUpType, int>()
    {
        { PowerUpType.TRIPLE_SHOT,  90 },
        { PowerUpType.SPEED_BOOST,  45 },
        { PowerUpType.SHIELD, 60 },
        { PowerUpType.AMMO, 125 },
        { PowerUpType.HEALTH, 20 },
        { PowerUpType.DOUBLE_HELIX, 30 },
        { PowerUpType.HOMING_LASER,  10 },
    };

    public static class Extensions
    {
        public static PowerUpType GetRandomPowerUp()
        {
            int randomIndex = UnityEngine.Random.Range(0, powerUpTypes.Length);
            return powerUpTypes[randomIndex];
        }

        public static PowerUpType GetWeightedRandomPowerUp ()
        {
            int[] weights = weightTable.Values.ToArray();
            int randomWeight = UnityEngine.Random.Range(0, weights.Sum());
            for (int i = 0;i < weights.Length; ++i)
            {
                randomWeight -= weights[i];
                if (randomWeight < 0)
                {
                    return powerUpTypes[i];
                }
            }

            // Should not default to this
            return PowerUpType.AMMO;
        }
    }

    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private PowerUpType powerUpID;

    private bool isAttracted = false;
    public bool IsAttracted
    {
        get => isAttracted;
        set { isAttracted = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttracted)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    public void MoveTowards (Vector3 otherPosition, float attractSpeed)
    {
        Vector3 direction = (otherPosition - transform.position).normalized;
        transform.Translate(direction * attractSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case PowerUpType.TRIPLE_SHOT:
                        player.TripleShotActive();
                        break;
                    case PowerUpType.SPEED_BOOST:
                        player.SpeedBoostActive();
                        break;
                    case PowerUpType.SHIELD:
                        player.ShieldActive();
                        break;
                    case PowerUpType.AMMO:
                        player.AddAmmo(15);
                        break;
                    case PowerUpType.HEALTH:
                        player.IncrementLives();
                        break;
                    case PowerUpType.DOUBLE_HELIX:
                        player.DoubleHelixActive();
                        break;
                    case PowerUpType.HOMING_LASER:
                        player.HomingLaserActive();
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
