using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBrain : BaseEnemyBrain
{
    private IMovable movable;
    private IShootable[] shootables;
    // TODO: separate HP component from Destructible component
    // have hp component as dependency of destructible component
    // create a different component responsible for destroying overall boss
    private Destructible[] destructibles;

    [SerializeField]
    private float shotFrequency = 4f;
    private float lastShotTime = 0f;

    [SerializeField]
    private Slider hpSlider;

    private int totalHp = 0;
    private int currentHp = 0;

    public float HpRatio
    {
        get
        {
            int newCurrentHp = 0;
            foreach (var destructible in destructibles)
            {
                newCurrentHp += destructible.Hp;
            }

            currentHp = newCurrentHp;

            if (totalHp <= 0)
            {
                return 0f;
            }

            return currentHp / (float)totalHp;
        }
    }

    private void Awake()
    {
        destructibles = GetComponentsInChildren<Destructible>();
        foreach(var destructible in destructibles)
        {
            totalHp += destructible.Hp;
        }
        currentHp = totalHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        movable = GetComponent<IMovable>();
        shootables = GetComponentsInChildren<IShootable>();
    }

    // Update is called once per frame
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

        // Update HP UI
        if (hpSlider != null)
        {
            hpSlider.value = HpRatio;
        }

    }
}
