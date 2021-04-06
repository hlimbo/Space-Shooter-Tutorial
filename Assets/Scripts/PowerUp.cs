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
    };

    public static class Extensions
    {
        public static PowerUpType GetRandomPowerUp()
        {
            var powerUpTypes = Enum.GetValues(typeof(PowerUpType))
                .Cast<PowerUpType>().ToArray();

            int randomIndex = UnityEngine.Random.Range(0, powerUpTypes.Length);
            return powerUpTypes[randomIndex];
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
                }
            }

            Destroy(gameObject);
        }
    }
}
