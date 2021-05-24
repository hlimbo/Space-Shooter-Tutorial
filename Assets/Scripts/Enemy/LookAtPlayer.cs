using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private float angleOffset = 90f;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //transform.LookAt(player.transform, Vector3.up);

            Vector3 diff = player.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetAngle * Mathf.Rad2Deg + angleOffset);

            // TODO: figure out why rotation isn't locked on Z only
            // Another way to rotate the enemy based on their local transforms.
            //transform.up = (player.transform.position - transform.position).normalized;
        }
        
    }
}
