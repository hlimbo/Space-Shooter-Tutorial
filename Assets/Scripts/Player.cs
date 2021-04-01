// Part of .NET Framework
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.5f;
    [SerializeField]
    private int speedMultiplier = 2;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleshotPrefab;
    [SerializeField]
    private float spawnOffset = 0.8f;
    [SerializeField]
    private float fireDelay = 0.5f;
    // Seconds until new laser shot can be fired
    private float newFireTime = -1f;
    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private bool isTripleShotEnabled = false;
    [SerializeField]
    private bool isSpeedBoostEnabled = false;
    [SerializeField]
    private bool isShieldEnabled = false;

    [SerializeField]
    private GameObject[] engines;

    private GameObject shield;

    [SerializeField]
    private int score = 0;

    private SpawnManager spawnManager;
    private UiManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("SpawnManager is null. Check the scene if game object with component SpawnManager exists.");
        }

        uiManager = FindObjectOfType<UiManager>();
        uiManager?.UpdateScore(score);

        if(uiManager == null)
        {
            Debug.LogError("UI Manager is null. Check the scene if game object with UiManager component exists");
        }

        shield = transform.Find("Shield").gameObject;
        if(!isShieldEnabled)
        {
            shield.SetActive(false);
        }
    }

    // Update is called once per frame
    // Update is called 1 frame or 60 frames per second
    // 1 meter per frame => 60 meters per second approx
    // Want to move player 1 meter per second instead of 1 meter per frame
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && newFireTime < Time.time)
        {
            newFireTime = Time.time + fireDelay;
            if(isTripleShotEnabled)
            {
                TripleShot();
            }
            else
            {
                FireLaser();
            }
        }
    }

    void CalculateMovement ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // top and bottom bounds ~ Alternatively, we can use Mathf.Clamp to keep movement limited between a min and max y value
        if (transform.position.y <= -3.6f)
        {
            transform.position = new Vector3(transform.position.x, -3.6f, transform.position.z);
        }
        else if (transform.position.y >= 5.8f)
        {
            transform.position = new Vector3(transform.position.x, 5.8f, transform.position.z);
        }

        // left and right wrap-around
        if (transform.position.x < -10f)
        {
            transform.position = new Vector3(10f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 10f)
        {
            transform.position = new Vector3(-10f, transform.position.y, transform.position.z);
        }
    }

    void FireLaser()
    {
        Vector3 laserOffset = Vector3.up * spawnOffset;
        Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
    }

    void TripleShot()
    {
        Instantiate(tripleshotPrefab, transform.position, Quaternion.identity);
    }

    public void Damage()
    {
        if(isShieldEnabled)
        {
            isShieldEnabled = false;
            shield.SetActive(isShieldEnabled);
            return;
        }

        // Randomly activate a broken engine upon getting damaged

        // Assumption: there will only be 2 engines in the array
        int engineToDamageIndex = Random.Range(0, engines.Length);
        if(engines[engineToDamageIndex].activeInHierarchy)
        {
            // damage the other engine instead
            engineToDamageIndex = (engineToDamageIndex + 1) % engines.Length;
        }

        engines[engineToDamageIndex].SetActive(true);

        --lives;
        uiManager.UpdateLives(lives);
        // Delete Player if no more lives left
        if (lives < 1)
        {
            spawnManager?.OnPlayerDeath();
            uiManager?.ToggleGameOver(true);
            Destroy(gameObject);
        }
    }

    public void EnableTripleShot()
    {
        isTripleShotEnabled = true;
    }

    public void TripleShotActive()
    {
        isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        isTripleShotEnabled = false;
    }

    public void SpeedBoostActive()
    {
        // Don't stack speed boosts if its already active
        if(!isSpeedBoostEnabled)
        {
            isSpeedBoostEnabled = true;
            speed *= speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        isSpeedBoostEnabled = false;
        speed /= speedMultiplier;
    }

    public void ShieldActive()
    {
        isShieldEnabled = true;
        shield.SetActive(isShieldEnabled);
    }

    public void AddScore(int points)
    {
        score += points;
        uiManager?.UpdateScore(score);
    }
}
