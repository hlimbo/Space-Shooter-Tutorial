using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeMagnitude;
    private float currentShakeMagnitude;
    [SerializeField]
    private float shakeDuration;

    private Vector3 originalPosition;

    private Coroutine shakeRef;

    private void Start()
    {
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Shake();
        }
    }

    public void Shake()
    {
        if(shakeRef == null)
        {
            shakeRef = StartCoroutine(ShakeRoutine());
        }
    }

    IEnumerator ShakeRoutine()
    {
        currentShakeMagnitude = shakeMagnitude;
        float startShakeTime = Time.time;
        float timeElapsed = Time.time - startShakeTime;
        while(timeElapsed < shakeDuration)
        {
            float newX = Random.Range(-1f, 1f) * currentShakeMagnitude;
            float newY = Random.Range(-.5f, .5f) * currentShakeMagnitude;
            transform.position = new Vector3(newX, newY, transform.position.z);
            currentShakeMagnitude = Mathf.MoveTowards(currentShakeMagnitude, 0f, (shakeMagnitude / shakeDuration) * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timeElapsed = Time.time - startShakeTime;
        }

        transform.position = originalPosition;

        shakeRef = null;
    }
}
