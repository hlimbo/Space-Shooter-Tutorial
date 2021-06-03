using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBrain : BaseEnemyBrain
{
    private IMovable movable;
    private IShootable[] shootables;
    private Collider2D[] colliders;
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField]
    private float shotFrequency = 4f;
    private float lastShotTime = 0f;

    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private int totalHp = 75;
    private int currentHp = 0;

    private Coroutine damageRoutineRef;
    private Renderer[] renderers;
    [SerializeField]
    private Material dmgMaterial;

    private UiManager uiManager;

    public float HpRatio
    {
        get
        {
            if (totalHp <= 0)
            {
                return 0f;
            }

            return currentHp / (float)totalHp;
        }
    }

    private void Awake()
    {
        movable = GetComponent<IMovable>();
        shootables = GetComponentsInChildren<IShootable>();
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        uiManager = FindObjectOfType<UiManager>();
    }

    private void Start()
    {
        lastShotTime = Time.time;
        currentHp = totalHp;
        hpSlider = GameObject.Find("BossHp")?.GetComponent<Slider>();
    }

    void Update()
    {
        movable?.Move(Time.deltaTime);
        if (Time.time - lastShotTime >= shotFrequency)
        {
            foreach (var shootable in shootables)
            {
                shootable?.FireShot();
            }
            lastShotTime = Time.time;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            if (currentHp > 0)
            {
                Destroy(other.gameObject);

                --currentHp;
                // Update HP UI
                if (hpSlider != null)
                {
                    hpSlider.value = HpRatio;
                }

                if (damageRoutineRef == null)
                {
                    damageRoutineRef = StartCoroutine(DamageEffectRoutine());
                }

                if (currentHp <= 0)
                {
                    ToggleColliders(false);
                    (movable as MonoBehaviour).enabled = false;
                    animator?.SetTrigger("isBossDead");
                    audioSource?.Play();
                    StartCoroutine(DelayDestruction());
                    uiManager?.DisplayWinnerText(true);
                }
            }
        }
    }

    IEnumerator DamageEffectRoutine()
    {
        var originalMats = new Material[renderers.Length];
        for(int i = 0;i < renderers.Length; ++i)
        {
            originalMats[i] = renderers[i].material;
        }

        SetMat(dmgMaterial);
        yield return new WaitForSeconds(0.15f);
        SetMats(renderers, originalMats);
        yield return new WaitForSeconds(0.15f);
        SetMat(dmgMaterial);
        yield return new WaitForSeconds(0.15f);
        SetMats(renderers, originalMats);

        damageRoutineRef = null;
    }

    private void SetMat(Material mat)
    {
        for (int i = 0;i < renderers.Length; ++i)
        {
            renderers[i].material = mat;
        }
    }

    private void SetMats(Renderer[] renderers, Material[] mats)
    {
        for(int i = 0;i < renderers.Length; ++i)
        {
            renderers[i].material = mats[i];
        }
    }

    private void ToggleColliders(bool toggle)
    {
        foreach(var collider in colliders)
        {
            collider.enabled = toggle;
        }
    }

    IEnumerator DelayDestruction()
    {
        yield return null;
        Destroy(gameObject, 2f);
    }
}
