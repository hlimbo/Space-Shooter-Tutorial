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
        Debug.Log("player: " + player);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //transform.LookAt(player.transform);

            Vector3 diff = player.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(diff.y, diff.x);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetAngle * Mathf.Rad2Deg + angleOffset);
        }
        
    }
}
