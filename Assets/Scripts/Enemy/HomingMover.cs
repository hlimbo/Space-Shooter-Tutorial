using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMover : MonoBehaviour, IMovable
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotateSpeed = 100f;

    [SerializeField]
    private GameObject target;
    private Rigidbody2D rb;

    private Vector3 direction;

    public GameObject Target => target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = transform.up;
    }

    public void SetTarget (GameObject target)
    {
        this.target = target;
    }

    // An alternative is to rotate ship using Cross Product and using rigidbody 2d
    public void Move(float deltaTime)
    {
        if (enabled && target != null)
        {
            direction = (target.transform.position - transform.position).normalized;

            // Why this code doesn't work ~ explain in an article TODO
            // This function causes ship to rotate towards a specific direction and will cause the ship to move away from player on rotation
            //float zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            //var rotateTarget = Quaternion.AngleAxis(zRotation, transform.forward);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotateTarget, deltaTime * 5f);

            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            transform.Translate(direction * speed * deltaTime, Space.World);
        }

        // Destroy game object if target becomes null
        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    // This function is added to allow this script to be enabled/disabled
    private void Update()
    {

    }
}
