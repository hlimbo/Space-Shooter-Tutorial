using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 8f;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > 7f || transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }
}
