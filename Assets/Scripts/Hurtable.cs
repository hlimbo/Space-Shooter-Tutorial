using System.Collections;
using UnityEngine;

public class Hurtable : MonoBehaviour
{
    [SerializeField]
    private int hp = 1;
    public int Hp => hp;

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Material dmgMaterial;

    private Coroutine damageRoutineRef;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator DamageEffectRoutine()
    {
        var originalMat = spriteRenderer.material;

        spriteRenderer.material = dmgMaterial;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material = originalMat;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material = dmgMaterial;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material = originalMat;

        damageRoutineRef = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            if (hp > 0)
            {
                --hp;
                if (damageRoutineRef == null)
                {
                    damageRoutineRef = StartCoroutine(DamageEffectRoutine());
                }
            }
        }
    }
}
