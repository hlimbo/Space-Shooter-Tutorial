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
    };

    private static PowerUpType[] powerUpTypes = Enum.GetValues(typeof(PowerUpType))
        .Cast<PowerUpType>().ToArray();

    private static Dictionary<PowerUpType, int> weightTable = new Dictionary<PowerUpType, int>()
    {
        { PowerUpType.TRIPLE_SHOT,  90 },
        { PowerUpType.SPEED_BOOST,  15 },
        { PowerUpType.SHIELD, 30 },
        { PowerUpType.AMMO, 105 },
        { PowerUpType.HEALTH, 60 },
        { PowerUpType.DOUBLE_HELIX, 60 },
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

    // Move down at speed of 3
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private PowerUpType powerUpID;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        
        // If below screen
        if(transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
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
                }
            }

            Destroy(gameObject);
        }
    }
}
