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
    private float boostMultiplier = 1.25f;
    private float currentBoostMultiplier = 1f;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleshotPrefab;
    [SerializeField]
    private GameObject doubleHelixPrefab;
    [SerializeField]
    private float spawnOffset = 0.8f;
    [SerializeField]
    private float fireDelay = 0.5f;
    // Seconds until new laser shot can be fired
    private float newFireTime = -1f;

    [SerializeField]
    private int maxLives = 3;
    [SerializeField]
    private int lives;

    [SerializeField]
    private bool isTripleShotEnabled = false;
    [SerializeField]
    private bool isSpeedBoostEnabled = false;
    [SerializeField]
    private bool isShieldEnabled = false;
    [SerializeField]
    private bool isDoubleHelixShotEnabled = false;

    [SerializeField]
    private GameObject[] engines;

    private GameObject shield;

    [SerializeField]
    private int score = 0;

    private SpawnManager spawnManager;
    private UiManager uiManager;

    [SerializeField]
    private AudioClip laserFire;
    [SerializeField]
    private AudioClip collectPowerUp;
    [SerializeField]
    private AudioClip noAmmoSound;
    private AudioSource audioSource;

    private Animator thrustAnimator;

    [SerializeField]
    private int startingAmmoCount = 15;
    private int currentAmmoCount;

    private CameraShake cameraShake;

    [SerializeField]
    [Range(0, 1f)]
    private float thrustGauge = 1f;
    private bool isBoostOnCooldown;

    private Camera mainCamera;
    private float camWidth, camHeight;

    private BoxCollider2D boxCollider;

    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.LogError("SpawnManager is null. Check the scene if game object with component SpawnManager exists.");
        }

        uiManager = FindObjectOfType<UiManager>();
        uiManager?.UpdateScore(score);
        currentAmmoCount = startingAmmoCount;
        uiManager?.UpdateAmmoText(currentAmmoCount);
        uiManager?.SetMaxAmmoText(startingAmmoCount);
        uiManager?.UpdateLives(lives);

        if(uiManager == null)
        {
            Debug.LogError("UI Manager is null. Check the scene if game object with UiManager component exists");
        }

        shield = transform.Find("Shield").gameObject;
        if(!isShieldEnabled)
        {
            shield.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();

        thrustAnimator = transform.Find("Thruster")?.GetComponent<Animator>();

        cameraShake = FindObjectOfType<CameraShake>();

        mainCamera = FindObjectOfType<Camera>();
        camHeight = 2f * mainCamera.orthographicSize;
        camWidth = camHeight * mainCamera.aspect;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    // Update is called 1 frame or 60 frames per second
    // 1 meter per frame => 60 meters per second approx
    // Want to move player 1 meter per second instead of 1 meter per frame
    void Update()
    {
        CalculateBoost();
        CalculateMovement();
        CheckMovementBounds();
        
        if (Input.GetKeyDown(KeyCode.Space) && newFireTime < Time.time)
        {
            newFireTime = Time.time + fireDelay;
            if(isTripleShotEnabled)
            {
                TripleShot();
                audioSource.PlayOneShot(laserFire);
            }
            else if(isDoubleHelixShotEnabled)
            {
                DoubleHelixShot();
                audioSource.PlayOneShot(laserFire);
            }
            else
            {
                if (currentAmmoCount > 0)
                {
                    FireLaser();
                    --currentAmmoCount;
                    uiManager?.UpdateAmmoText(currentAmmoCount);
                    audioSource.PlayOneShot(laserFire);
                }
                else
                {
                    audioSource.PlayOneShot(noAmmoSound);
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Powerup"))
        {
            audioSource.PlayOneShot(collectPowerUp);
        }
        else if (other.tag.Equals("EnemyLaser"))
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

    void CalculateBoost ()
    {
        if (Input.GetAxisRaw("ThrustBoost") == 1 && !isBoostOnCooldown)
        {
            currentBoostMultiplier = boostMultiplier;
            thrustAnimator?.SetBool("isBoosting", true);
            thrustGauge -= Time.deltaTime;
            uiManager.SetThrustFill(thrustGauge);

            if(thrustGauge <= 0f)
            {
                isBoostOnCooldown = true;
            }
        }
        else
        {
            if(thrustGauge < 1f)
            {
                thrustGauge += Time.deltaTime;
            }

            if(thrustGauge >= 1f && isBoostOnCooldown)
            {
                thrustGauge = 1f;
                isBoostOnCooldown = false;
            }

            uiManager.SetThrustFill(thrustGauge);
            currentBoostMultiplier = 1f;
            thrustAnimator?.SetBool("isBoosting", false);
        }
    }

    void CalculateMovement ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        transform.Translate(direction * speed * currentBoostMultiplier * Time.deltaTime);
    }

    void CheckMovementBounds ()
    {
        float maxY = mainCamera.transform.position.y + (camHeight / 2f) - (boxCollider.size.y / 2f);
        float minY = mainCamera.transform.position.y - (camHeight / 2f) + (boxCollider.size.y / 2f);
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.y);
        }
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.y);
        }

        // Left and right wrap-around
        float minX = mainCamera.transform.position.x - (camWidth / 2f);
        float maxX = mainCamera.transform.position.x + (camWidth / 2f);
        if(transform.position.x < minX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        else if(transform.position.x > maxX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
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

    void DoubleHelixShot()
    {
        Vector3 laserOffset = Vector3.up * spawnOffset;
        Instantiate(doubleHelixPrefab, transform.position + laserOffset, Quaternion.identity);
    }

    public void Damage()
    {
        if(isShieldEnabled)
        {
            isShieldEnabled = false;
            shield.SetActive(isShieldEnabled);
            return;
        }

        ToggleEngineDamage(true);
        cameraShake.Shake();

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

    public void DoubleHelixActive()
    {
        isDoubleHelixShotEnabled = true;
        StartCoroutine(DoubleHelixPowerDownRoutine());
    }

    IEnumerator DoubleHelixPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        isDoubleHelixShotEnabled = false;
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

    public void AddAmmo(int ammo)
    {
        if(currentAmmoCount + ammo < startingAmmoCount)
        {
            currentAmmoCount += ammo;
        }
        else if(Mathf.Abs(startingAmmoCount - currentAmmoCount) < startingAmmoCount)
        {
            // give remaining bullets instead of fixed amount
            currentAmmoCount += Mathf.Abs(startingAmmoCount - currentAmmoCount);
        }

        uiManager?.UpdateAmmoText(currentAmmoCount);
    }

    public void IncrementLives()
    {
        if(lives + 1 <= maxLives)
        {
            ++lives;
            uiManager?.UpdateLives(lives);
            ToggleEngineDamage(false);
        }
    }

    // Used to apply damage / repair vfx
    private void ToggleEngineDamage(bool toggle)
    {
        // Assumption: there will only be 2 engines in the array
        int engineToDamageIndex = Random.Range(0, engines.Length);
        // if randomly chosen engine is already damaged or repaired, then repair the other engine instead
        if (engines[engineToDamageIndex].activeInHierarchy == toggle)
        {
            engineToDamageIndex = (engineToDamageIndex + 1) % engines.Length;
        }

        engines[engineToDamageIndex].SetActive(toggle);
    }
}
